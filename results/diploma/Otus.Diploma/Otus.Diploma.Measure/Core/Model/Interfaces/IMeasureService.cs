namespace Measure.Core.Model.Interfaces
{
    public interface IMeasureService
    {
        Task<IEnumerable<Measure>?> GetMeasuresAsync(ulong deviceId, bool useReplica, CancellationToken cancellationToken);
    }
}
