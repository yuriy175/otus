using Microsoft.IdentityModel.Tokens;
using NRedisStack;
using NRedisStack.RedisStackCommands;
using Profile.Infrastructure.Repositories.Interfaces;
using StackExchange.Redis;

namespace Profile.Infrastructure.Caches
{
    public class RedisService : ICacheService
    {
        private readonly static string? _host = Environment.GetEnvironmentVariable("REDIS_HOST");
        public RedisService()
        {
            /*var configOption = new ConfigurationOptions();
            //configOption.EndPoints.Add("127.0.0.1:6379");
            configOption.EndPoints.Add("172.18.0.5:6379");
            configOption.AllowAdmin = true;
            //configOption.AbortOnConnectFail = false;
            //var redis2 = ConnectionMultiplexer.Connect(configOption);
            //ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("redis-server");
            IDatabase db = redis.GetDatabase();

            var hash = new HashEntry[] {
    new HashEntry("name", "John"),
    new HashEntry("surname", "Smith"),
    new HashEntry("company", "Redis"),
    new HashEntry("age", new Random(100).Next().ToString()),
    };
            db.HashSet("user-session:123", hash);

            var hashFields = db.HashGetAll("user-session:123");
            Console.WriteLine(String.Join("; ", hashFields));

            int y = 0;*/
        }
        private Random _rnd = new Random(100);
        public void GetNext()
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(_host);// "redis-server");
            IDatabase db = redis.GetDatabase();

            var hash = new HashEntry[] {
    new HashEntry("name", "John"),
    new HashEntry("surname", "Smith"),
    new HashEntry("company", "Redis"),
    new HashEntry("age", _rnd.Next().ToString()),
    };
            db.HashSet("user-session:123", hash);

            var hashFields = db.HashGetAll("user-session:123");
            Console.WriteLine(String.Join("; ", hashFields));
        }
    }
}
