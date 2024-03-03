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
        private readonly MQSender _mqSender;
        private readonly MQSender2 _mqSender2;

        private static readonly ConcurrentDictionary<ulong, (TaskCompletionSource Task, IEnumerable<Model.Measure> Measures)> _tasks = 
            new ConcurrentDictionary<ulong, (TaskCompletionSource, IEnumerable<Model.Measure>)> { };
        private static readonly SortedList<ulong, ulong> _remList = new SortedList<ulong, ulong> { };
        private static readonly SortedList<ulong, int> _rcvCntList = new SortedList<ulong, int> { };
        private static readonly SortedList<ulong, List<IEnumerable<Model.Measure>>> _rcvMeasList = new SortedList<ulong, List<IEnumerable<Model.Measure>>> { };
        private bool _shouldFail = false;
        public MeasureService(
            IMapper mapper, 
            IGrpcChannelsProvider channelsProvider,
            MQSender mqSender,
            MQSender2 mqSender2)
        {
            _mapper = mapper;
            _channelsProvider = channelsProvider;
            _mqSender = mqSender;
            _mqSender2 = mqSender2;

            _mqSender.CreateDirectReceiver(async (data) =>
            {
                if (_shouldFail)
                {
                    var text = Encoding.UTF8.GetString(data);
                    var mesuares = JsonSerializer.Deserialize<IEnumerable<Model.Measure>>(text);
                    if (mesuares == null)
                    {
                        return;
                    }
                    var deviceId = mesuares.First().DeviceId;
                    _tasks.TryGetValue(deviceId, out (TaskCompletionSource Task, IEnumerable<Model.Measure> Measures) value);
                    _tasks.TryUpdate(deviceId, (value.Task, mesuares), value);
                    _rcvCntList[deviceId] = !_rcvCntList.ContainsKey(deviceId) ? 0 : ++_rcvCntList[deviceId];
                    if (!_rcvMeasList.ContainsKey(deviceId))
                    {
                        _rcvMeasList[deviceId] = new List<IEnumerable<Model.Measure>> { mesuares };
                    }
                    else
                    {
                        _rcvMeasList[deviceId].Add(mesuares);
                    }
                    value.Task.TrySetResult();
                }

                //var measures = await _measureRepository.GetMeasuresAsync(message.DeviceId, message.UseReplica, CancellationToken.None);
            });

            _mqSender2.CreateDirectReceiver(async (data) =>
            {
                if (_shouldFail)
                {
                    var text = Encoding.UTF8.GetString(data);
                    var mesuares = JsonSerializer.Deserialize<IEnumerable<Model.Measure>>(text);
                    if (mesuares == null)
                    {
                        return;
                    }
                    var deviceId = mesuares.First().DeviceId;
                    _tasks.TryGetValue(deviceId, out (TaskCompletionSource Task, IEnumerable<Model.Measure> Measures) value);
                    _tasks.TryUpdate(deviceId, (value.Task, mesuares), value);
                    _rcvCntList[deviceId] = !_rcvCntList.ContainsKey(deviceId) ? 0 : ++_rcvCntList[deviceId];
                    if (!_rcvMeasList.ContainsKey(deviceId))
                    {
                        _rcvMeasList[deviceId] = new List<IEnumerable<Model.Measure>> { mesuares };
                    }
                    else
                    {
                        _rcvMeasList[deviceId].Add(mesuares);
                    }
                    value.Task.TrySetResult();
                }

                //var measures = await _measureRepository.GetMeasuresAsync(message.DeviceId, message.UseReplica, CancellationToken.None);
            });
        }

        public async Task<IEnumerable<MeasureDto>?> GetMeasuresAsync(ulong deviceId, bool useReplica, CancellationToken cancellationToken)
        {
            //var measureClient = new MeasureClient(_channelsProvider.GetMeasureChannel());
            //var reply = await measureClient.GetMeasuresAsync(new GetMeasuresRequest { DeviceId = deviceId, UseReplica = useReplica }) ;

            //return reply.Measures.Select(m => _mapper.Map<MeasureDto>(m));

            var measureClient = new MeasureClient(_channelsProvider.GetMeasureChannel());
            var measureClient2 = new MeasureClient(_channelsProvider.GetMeasureChannel2());
            var reply = deviceId % 2== 0 ? 
                await measureClient.GetMeasuresAsync(new GetMeasuresRequest { DeviceId = deviceId, UseReplica = useReplica }) :
                await measureClient2.GetMeasuresAsync(new GetMeasuresRequest { DeviceId = deviceId, UseReplica = useReplica });

            return reply.Measures.Select(m => _mapper.Map<MeasureDto>(m));
            
        }

        private List<ulong> _items = new List<ulong> { };

        public async Task<IEnumerable<MeasureDto>?> GetQueuedMeasuresAsync(ulong deviceId, bool useReplica, CancellationToken cancellationToken)
        {
            _shouldFail = true;
            var task = new TaskCompletionSource();

            //if(_tasks.TryGetValue(deviceId, out (TaskCompletionSource Task, IEnumerable<Model.Measure> Measures) value2))
            //{
            //    var t = 0;
            //}
            if (_items.Contains(deviceId))
            {
                var i = 0;
            }
            _items.Add(deviceId);
            _ = _tasks.TryAdd(deviceId, (task, null));
            //if (deviceId % 2 == 0) {
                _mqSender.SendRequest(deviceId, new MeasureRequestMessage { DeviceId = deviceId, UseReplica = useReplica });
            //} else
            //{
            //    //_mqSender2.SendRequest(deviceId, new MeasureRequestMessage { DeviceId = deviceId, UseReplica = useReplica });
            //}

            await task.Task;

            _tasks.TryRemove(deviceId, out (TaskCompletionSource, IEnumerable<Model.Measure> Measures) value);
            //Console.WriteLine(deviceId);
            _remList.TryAdd(deviceId, deviceId);
            var w = value.Measures.Select(m => _mapper.Map<Model.Measure, MeasureDto>(m)).ToList();
            return w;
        }
    }
}
