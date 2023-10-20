using Microsoft.Extensions.Options;
using Npgsql;
using System.Data;
using System.Data.Common;
using System.Runtime;

namespace Profile.Infrastructure.Repositories
{
    public class DataContext
    {
        private readonly bool _canUseReadConnection;

        private readonly string _connection;
        private readonly string _readConnection;

        public DataContext(string connection, string readConnection)
        {
            _connection = connection;
            _readConnection = readConnection;

            try
            {
                using var conn = new NpgsqlConnection(_readConnection);
                conn.Open();
                _canUseReadConnection = true;
            }
            catch (Exception ex)
            {
                _canUseReadConnection = false;
            }
        }

        public IDbConnection CreateConnection()
        {
            return new NpgsqlConnection(_connection);
        }

        public IDbConnection CreateReadConnection()
        {
            return new NpgsqlConnection(_canUseReadConnection ? _readConnection : _connection);
        }
    }
}
