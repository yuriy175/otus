using SocialNetApp.Core.Model;

namespace Profile.Infrastructure.Caches
{
    public interface ICacheService
    {
        Task AddPostAsync(uint userId, Post post);
        Task<IEnumerable<Post>> GetPostsAsync(uint userId, uint offset, uint limit);
        Task WarmupCacheAsync(uint userId, IEnumerable<Post> posts);
    }
}
