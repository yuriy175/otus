using Common.MQ.Core.Model.Interfaces;
using Posts.Core.Model.Interfaces;
using Posts.Infrastructure.Caches;
using Posts.Infrastructure.Repositories.Interfaces;
using SocialNetApp.Core.Model;

namespace Profile.Core.Services
{
    public class PostsService : IPostsService
    {        
        private readonly IPostsRepository _postsRepository;
        private readonly ICacheService _cacheService;
        private readonly IFriendsRepository _friendsRepository;
        private readonly IMQSender _mqSender;
        
        private readonly static uint _cacheItemsCount = Convert.ToUInt32(Environment.GetEnvironmentVariable("CACHE_ITEMS_COUNT"));

        public PostsService(
            IPostsRepository postsRepository,
            IFriendsRepository friendsRepository,
            ICacheService cacheService,
            IMQSender mqSender)
        {
            _postsRepository = postsRepository;
            _friendsRepository = friendsRepository;
            _cacheService = cacheService;
            _mqSender = mqSender;
        }

        public async Task<int> CreatePostAsync(uint userId, string text, CancellationToken cancellationToken)
        {
            var post = await _postsRepository.AddPostAsync(userId, text, cancellationToken);
            _mqSender.SendPost(userId, text);
            var subscriberIds = await _friendsRepository.GetSubscriberIdsAsync(userId, cancellationToken);
            foreach (var subscriberId in subscriberIds)
            {
                await _cacheService.AddPostAsync(Convert.ToUInt32(subscriberId), post);
            }

            return 1;
        }

        public Task<int> UpdatePostAsync(uint userId, uint postId, string text, CancellationToken cancellationToken) =>
            _postsRepository.UpdatePostAsync(userId, postId, text, cancellationToken);

        public Task<int> DeletePostAsync(uint userId, uint postId, CancellationToken cancellationToken) =>
            _postsRepository.DeletePostAsync(userId, postId, cancellationToken);

        public Task<Post> GetPostAsync(uint userId, uint postId, CancellationToken cancellationToken)
        {
            return _postsRepository.GetPostAsync(userId, postId, cancellationToken);
        }

        public async Task<IEnumerable<Post>> FeedPostsAsync(uint userId, uint offset, uint limit, CancellationToken cancellationToken)
        {
            var posts = await _cacheService.GetPostsAsync(userId, offset, limit);
            if (!posts.Any())
            {
                posts = await _postsRepository.GetLatestFriendsPostsAsync(userId, _cacheItemsCount, cancellationToken);
                await _cacheService.WarmupCacheAsync(userId, posts);
                posts = await _cacheService.GetPostsAsync(userId, offset, limit);
            }

            return posts;
        }
    }
}
