using Bff.API.Dtos;

namespace Bff.Infrastructure.gRpc.Services.Interfaces
{
    public interface IMeasureService
    {
        Task<IEnumerable<MeasureDto>?> GetMeasuresAsync(ulong deviceId, bool useReplica, CancellationToken cancellationToken);
        Task<IEnumerable<MeasureDto>?> GetQueuedMeasuresAsync(ulong deviceId, bool useReplica, CancellationToken cancellationToken);
    }
}
