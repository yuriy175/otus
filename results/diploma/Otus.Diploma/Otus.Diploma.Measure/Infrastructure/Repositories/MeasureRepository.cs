using Dapper;
using Measure.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using Model = Measure.Core.Model;

namespace Measure.Infrastructure.Repositories
{
    public class MeasureRepository : IMeasureRepository
    {
        private DataContext _context;

        public MeasureRepository(DataContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<Model.Measure>?> GetMeasuresAsync(ulong deviceId, bool useReplica, CancellationToken cancellationToken)
        {
            using var connection = _context.CreateReadConnection(useReplica);
            var sql = "SELECT id, device_id as deviceId, device_mod_id, type, date, value " +
                        "FROM public.measure " +
                        "where device_id = @deviceId and date = (select max(date) from public.measure where device_id = @deviceId)";

            return await connection.QueryAsync<Model.Measure>(new CommandDefinition(
                sql,
                new { deviceId = (long)deviceId },
                cancellationToken: cancellationToken));
        }
    }
}
