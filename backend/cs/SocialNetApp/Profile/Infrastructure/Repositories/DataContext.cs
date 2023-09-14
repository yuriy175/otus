using Microsoft.Extensions.Options;
using Npgsql;
using System.Data;
using System.Data.Common;
using System.Runtime;

namespace Profile.Infrastructure.Repositories
{
    public class DataContext
    {
        private readonly string _connection;
        public DataContext(string connection)
        {
            _connection = connection;
        }

        public IDbConnection CreateConnection()
        {
            return new NpgsqlConnection(_connection);
        }
    }
}
