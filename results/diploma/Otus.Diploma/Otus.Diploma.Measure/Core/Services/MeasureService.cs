using Common.MQ.Core.Model.Interfaces;
using Common.MQ.Core.Model.Types;
using Common.MQ.Core.Services;
using Measure.Core.Model.Interfaces;
using Measure.Infrastructure.Repositories.Interfaces;
using System.Text;
using System.Text.Json;
using System.Threading;
using Model = Measure.Core.Model;

namespace Measure.Core.Services
{
    public class MeasureService : IMeasureService
    {        
        private readonly IMeasureRepository _measureRepository;
        private readonly IMQueue _mqReceiver;

        public MeasureService(
            IMeasureRepository measureRepository,
            IMQueue mqReceiver)
        {
            _measureRepository = measureRepository;
            _mqReceiver = mqReceiver;

            _mqReceiver.CreateDirectReceiver(async (data) =>
            {
                var text = Encoding.UTF8.GetString(data);
                var message = JsonSerializer.Deserialize<MeasureRequestMessage>(text);
                if (message == null)
                {
                    return;
                }

                var measures = await _measureRepository.GetMeasuresAsync(message.DeviceId, message.UseReplica, CancellationToken.None);
                _mqReceiver.SendRequest(measures);
            });
        }

        public async Task<IEnumerable<Model.Measure>?> GetMeasuresAsync(ulong deviceId, bool useReplica, CancellationToken cancellationToken)
        {
            var measures = await _measureRepository.GetMeasuresAsync(deviceId, useReplica, cancellationToken);
            return measures;
        }
    }
}
