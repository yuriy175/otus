using AutoMapper;
using Bff.API.Dtos;
using Bff.Infrastructure.gRpc.Services.Interfaces;
using Bff.Infrastructure.Services.Interfaces;
using MeasureGrpc;
using Grpc.Net.Client;
using RabbitMQ.Client;
using System.Diagnostics;
using static MeasureGrpc.Measure;
using Common.MQ.Core.Model.Interfaces;
using Common.MQ.Core.Model.Types;
using Common.MQ.Core.Services;
using System.Text;
using System.Text.Json;
using Model = Measure.Core.Model;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Bff.Infrastructure.gRpc.Services
{
    public class MeasureService : IMeasureService
    {
        private readonly IMapper _mapper;
        private readonly IGrpcChannelsProvider _channelsProvider;
        private readonly IMQueue _mqSender;

        private readonly ConcurrentDictionary<ulong, (TaskCompletionSource Task, IEnumerable<Model.Measure> Measures)> _tasks = 
            new ConcurrentDictionary<ulong, (TaskCompletionSource, IEnumerable<Model.Measure>)> { };

        public MeasureService(
            IMapper mapper, 
            IGrpcChannelsProvider channelsProvider,
            IMQueue mqSender)
        {
            _mapper = mapper;
            _channelsProvider = channelsProvider;
            _mqSender = mqSender;

            _mqSender.CreateDirectReceiver(async (data) =>
            {
                var text = Encoding.UTF8.GetString(data);
                var mesuares = JsonSerializer.Deserialize<IEnumerable<Model.Measure>>(text);
                if (mesuares == null)
                {
                    return;
                }
                var deviceId = mesuares.First().DeviceId;
                _tasks.AddOrUpdate(deviceId, (null, null), (k, p) =>
                {
                    return (p.Task, mesuares);
                });
                _tasks.TryGetValue(deviceId, out (TaskCompletionSource Task, IEnumerable<Model.Measure> Measures) value);
                value.Task.TrySetResult();

                //var measures = await _measureRepository.GetMeasuresAsync(message.DeviceId, message.UseReplica, CancellationToken.None);
            });
        }

        public async Task<IEnumerable<MeasureDto>?> GetMeasuresAsync(ulong deviceId, bool useReplica, CancellationToken cancellationToken)
        {
            var measureClient = new MeasureClient(_channelsProvider.GetMeasureChannel());
            var reply = await measureClient.GetMeasuresAsync(new GetMeasuresRequest { DeviceId = deviceId, UseReplica = useReplica });

            return reply.Measures.Select(m => _mapper.Map<MeasureDto>(m));
        }

        public async Task<IEnumerable<MeasureDto>?> GetQueuedMeasuresAsync(ulong deviceId, bool useReplica, CancellationToken cancellationToken)
        {
            var task = new TaskCompletionSource();           

            _tasks.AddOrUpdate(deviceId, (task, null), (k, p) => (task, null));
            _mqSender.SendRequest(new MeasureRequestMessage { DeviceId = deviceId, UseReplica = useReplica });

            await task.Task;

            _tasks.TryRemove(deviceId, out (TaskCompletionSource, IEnumerable<Model.Measure> Measures) value);
            return value.Measures.Select(m => _mapper.Map<MeasureDto>(m)); ;
        }
    }
}
