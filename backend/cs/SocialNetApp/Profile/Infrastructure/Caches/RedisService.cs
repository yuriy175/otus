using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using NRedisStack;
using NRedisStack.RedisStackCommands;
using Profile.Infrastructure.Repositories.Interfaces;
using SocialNetApp.Core.Model;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Text.Json;

namespace Profile.Infrastructure.Caches
{
    public class RedisService : ICacheService, IAsyncDisposable
    {
        private readonly static string? _host = Environment.GetEnvironmentVariable("REDIS_HOST");
        private readonly static uint _cacheItemsCount = Convert.ToUInt32(Environment.GetEnvironmentVariable("CACHE_ITEMS_COUNT"));

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

        public async Task AddPostAsync(uint userId, Post post)
        {
            var userKey = $"user:{userId}";
            if (_db is null)
            {
                throw new ApplicationException("Пустой redis db");
            }

            //await _db.ListLeftPushAsync(userKey, new RedisValue(JsonSerializer.Serialize<Post>(post)));
            //await _db.ListTrimAsync(userKey, 0, _cacheItemsCount-1);
            var args = new object[] { 
                "addpost", 
                1, userKey,
                new RedisValue(JsonSerializer.Serialize<Post>(post)), _cacheItemsCount-1};
            var count = await _db.ExecuteAsync("FCALL", args);
        }

        public async Task<IEnumerable<Post>> GetPostsAsync(uint userId, uint offset, uint limit)
        {
            var userKey = $"user:{userId}";
            if (_db is null)
            {
                throw new ApplicationException("Пустой redis db");
            }

            //var values = await _db.ListRangeAsync(userKey, offset, limit);
            //return values.Select(v => JsonSerializer.Deserialize<Post>(v)).ToArray();
            var values = await _db.ExecuteAsync("FCALL", new object[] { "getposts", 1, userKey, offset, limit });
            if(values.Type == ResultType.MultiBulk)
            {
                return ((RedisResult[])values!).Select(v => JsonSerializer.Deserialize<Post>(v.ToString()!)).ToArray();
            }
            return Array.Empty<Post>();
        }

        public async Task WarmupCacheAsync(uint userId, IEnumerable<Post> posts)
        {
            var userKey = $"user:{userId}";
            
            if (_db is null)
            {
                throw new ApplicationException("Пустой redis db");
            }

            await _db.KeyExpireAsync(userKey, TimeSpan.FromHours(24));
            //var items = posts.Select(p =>
            //{
            //    var json = JsonSerializer.Serialize<Post>(p);
            //    return new RedisValue(json);
            //}).ToArray();

            //await _db.ListRightPushAsync(userKey, items);
            var args = new object[] { "warmupposts", 1, userKey }
                .Union(posts.Select(p =>JsonSerializer.Serialize<Post>(p)))
                .ToArray();
            var count = await _db.ExecuteAsync("FCALL", args);

            return;
        }

        public ValueTask DisposeAsync()
        {
            using (_redis) { }

            return ValueTask.CompletedTask;
        }
    }
}
