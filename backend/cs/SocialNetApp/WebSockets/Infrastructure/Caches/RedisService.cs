using NRedisStack;
using StackExchange.Redis;
using System.Text.Json;

namespace WebSockets.Infrastructure.Caches
{
    public class RedisService : ICacheService, IAsyncDisposable
    {
        private readonly static string? _host = Environment.GetEnvironmentVariable("REDIS_HOST");

        private readonly IDatabase? _db = default;
        private readonly ConnectionMultiplexer _redis = default!;

        public RedisService()
        {
            if (string.IsNullOrEmpty(_host))
            {
                throw new ApplicationException("Пустой redis host");
            }
            _redis = ConnectionMultiplexer.Connect(_host!);
            if (_redis is null)
            {
                throw new ApplicationException("Пустой redis");
            }

            _db = _redis.GetDatabase();
            if (_db is null)
            {
                throw new ApplicationException("Пустой redis db");
            }
        }

        public async Task UpsertUserWebSocketAsync(uint userId, string hostName, string port)
        {
            var userKey = $"user:{userId}_ws";
            if (_db is null)
            {
                throw new ApplicationException("Пустой redis db");
            }
            await _db.StringSetAsync(userKey, $"{hostName}_{port}");
        }

        public ValueTask DisposeAsync()
        {
            using (_redis) { }

            return ValueTask.CompletedTask;
        }
    }
}
