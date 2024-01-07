using Posts.Core.Model;

namespace Posts.Infrastructure.Repositories.Interfaces
{
    public interface IPostsRepository
    {
        Task<Post> AddPostAsync(uint userId, string text, CancellationToken cancellationToken);
        Task<int> UpdatePostAsync(uint userId, uint postId, string text, CancellationToken cancellationToken);
        Task<int> DeletePostAsync(uint userId, uint postId, CancellationToken cancellationToken);
        Task<Post> GetPostAsync(uint userId, uint postId, CancellationToken cancellationToken);
        Task<IEnumerable<Post>> GetPostsAsync(uint userId, uint offset, uint limit, CancellationToken cancellationToken);
        Task<IEnumerable<Post>> GetLatestPostsAsync(uint userId, uint limit, CancellationToken cancellationToken);
        Task<IEnumerable<Post>> GetLatestFriendsPostsAsync(uint userId, uint limit, CancellationToken cancellationToken);
    }
}
