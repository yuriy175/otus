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
        private readonly string _readConnectionString;
        private readonly string _readConnectionString2;
        private readonly NpgsqlConnection _readConnection;
        private readonly NpgsqlConnection _readConnection2;
        public DataContext(string readConnectionString, string readConnectionString2)
        {
            _readConnectionString = readConnectionString;
            _readConnectionString2 = readConnectionString2;
            _readConnection = new NpgsqlConnection(_readConnectionString);
            _readConnection2 = new NpgsqlConnection(_readConnectionString2);
        }


        public IDbConnection CreateReadConnection(bool useReplica)
        {
            //return new NpgsqlConnection(_readConnectionString);
            return new NpgsqlConnection(count++ % (useReplica ? 2 : 1) == 0 ? _readConnectionString : _readConnectionString2);
            //return new NpgsqlConnection(count % 2 == 0 ? _readConnection : _readConnection2);
            //return (count++ % (useReplica ? 2 : 1) == 0) ? _readConnection : _readConnection2;
        }
    }
}
