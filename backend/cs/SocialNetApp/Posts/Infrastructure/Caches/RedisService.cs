using NRedisStack;
using Posts.Core.Model;
using StackExchange.Redis;
using System.Text.Json;

namespace Posts.Infrastructure.Caches
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

            await _db.ListLeftPushAsync(userKey, new RedisValue(JsonSerializer.Serialize<Post>(post)));
            await _db.ListTrimAsync(userKey, 0, _cacheItemsCount-1);
        }

        public async Task<IEnumerable<Post>> GetPostsAsync(uint userId, uint offset, uint limit)
        {
            var userKey = $"user:{userId}";
            if (_db is null)
            {
                throw new ApplicationException("Пустой redis db");
            }

            var values = await _db.ListRangeAsync(userKey, offset, limit);
            return values.Select(v => JsonSerializer.Deserialize<Post>(v)).ToArray();
        }

        public async Task WarmupCacheAsync(uint userId, IEnumerable<Post> posts)
        {
            var userKey = $"user:{userId}";
            
            if (_db is null)
            {
                throw new ApplicationException("Пустой redis db");
            }

            await _db.KeyExpireAsync(userKey, TimeSpan.FromHours(24));
            var items = posts.Select(p =>
            {
                var json = JsonSerializer.Serialize<Post>(p);
                return new RedisValue(json);
            }).ToArray();

            await _db.ListRightPushAsync(userKey, items);
        }
        /*
         * 
        public Task<bool> AddPostAsync(Post post)
        {
            var authorKey = $"author:{post.AuthorId}";
            if (_db is null)
            {
                throw new ApplicationException("Пустой redis db");
            }
            
            var value = new RedisValue( post.Message );

            return _db.SortedSetAddAsync(authorKey, value, post.Id);
        }

        public async Task<IEnumerable<Post>> GetPostsAsync(uint userId, uint offset, uint limit)
        {
            var allKey = $"author:all";
            var y = await _db.SortedSetCombineAndStoreAsync(SetOperation.Union, allKey, 
                new []{"author:1218263", "author:1218274", "author:1218275" }.Select(i => new RedisKey(i)).ToArray(), 
                aggregate: Aggregate.Max,
                weights: new[] {1.0,1.0, 1.0});

            var authorKey = $"author:{userId}";
            if (_db is null)
            { 
                throw new ApplicationException("Пустой redis db");
            }

            var values2 = await _db.SortedSetRangeByScoreWithScoresAsync("author:1218274");

            var values = await _db.SortedSetRangeByScoreWithScoresAsync(allKey,
                order: Order.Descending,
                skip: offset,
                take: limit);// authorKey);
            return values.Select(v => new Post { AuthorId = userId, Id = Convert.ToUInt32(v.Score), Message = v.Element.ToString()});
        }
        
        public Task WarmupCacheAsync(uint userId, IEnumerable<Post> posts)
        {
            var authorKey = $"author:{userId}";
            if (_db is null)
            {
                throw new ApplicationException("Пустой redis db");
            }

            return _db.SortedSetAddAsync(authorKey, posts.Select(p => new SortedSetEntry(new RedisValue(p.Message), p.Id)).ToArray());
        }*/

        public ValueTask DisposeAsync()
        {
            using (_redis) { }

            return ValueTask.CompletedTask;
        }
    }
}
