using Common.MQ.Core.Model.Interfaces;
using MediatR;
using Posts.Infrastructure.Caches;
using Posts.Infrastructure.Repositories.Interfaces;
using SocialNetApp.Core.Model;
using System.Collections.Generic;

namespace Posts.Application.Queries.Posts
{
    public class FeedPostsQuery : IRequest<IEnumerable<Post>>
    {
        private readonly static uint _cacheItemsCount = Convert.ToUInt32(Environment.GetEnvironmentVariable("CACHE_ITEMS_COUNT"));

        public uint UserId { get; init; }
        public uint Offset { get; init; }
        public uint Limit { get; init; }

        public class FeedPostsQueryHandler : IRequestHandler<FeedPostsQuery, IEnumerable<Post>>
        {
            private readonly IPostsRepository _postsRepository;
            private readonly ICacheService _cacheService;
            public FeedPostsQueryHandler(
                IPostsRepository postsRepository,
                ICacheService cacheService)
            {
                _postsRepository = postsRepository;
                _cacheService = cacheService;
            }
            public async Task<IEnumerable<Post>> Handle(FeedPostsQuery query, CancellationToken cancellationToken)
            {
                var posts = await _cacheService.GetPostsAsync(query.UserId, query.Offset, query.Limit);
                if (!posts.Any())
                {
                    posts = await _postsRepository.GetLatestFriendsPostsAsync(query.UserId, _cacheItemsCount, cancellationToken);
                    await _cacheService.WarmupCacheAsync(query.UserId, posts);
                    posts = await _cacheService.GetPostsAsync(query.UserId, query.Offset, query.Limit);
                }

                return posts;
            }
        }
    }
}
