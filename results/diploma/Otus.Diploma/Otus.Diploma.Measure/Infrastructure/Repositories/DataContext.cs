using Microsoft.Extensions.Options;
using Npgsql;
using System.Data;
using System.Data.Common;
using System.Runtime;

namespace Measure.Infrastructure.Repositories
{
    public class DataContext
    {
        private static int count = 0;
        private readonly string _readConnection;
        private readonly string _readConnection2;
        public DataContext(string readConnection, string readConnection2)
        {
            _readConnection = readConnection;
            _readConnection2 = readConnection2;
        }


        public IDbConnection CreateReadConnection(bool useReplica)
        {
            return new NpgsqlConnection(count++ % (useReplica ? 2 : 1) == 0 ? _readConnection : _readConnection2);
            //return new NpgsqlConnection(count % 2 == 0 ? _readConnection : _readConnection2);
        }
    }
}
