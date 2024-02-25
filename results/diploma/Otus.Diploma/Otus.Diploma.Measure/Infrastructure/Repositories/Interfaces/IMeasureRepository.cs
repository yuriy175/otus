using Dapper;
using Model = Measure.Core.Model;

namespace Measure.Infrastructure.Repositories.Interfaces
{
    public interface IMeasureRepository
    {
        Task<IEnumerable<Model.Measure>?> GetMeasuresAsync(ulong deviceId, bool useReplica, CancellationToken cancellationToken);
    }
}
