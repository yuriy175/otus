using SocialNetApp.Core.Model;

namespace Profile.Core.Model.Interfaces
{
    public interface IPostsService
    {
        Task<int> CreatePostAsync(uint userId, string text, CancellationToken cancellationToken);
        Task<int> UpdatePostAsync(uint userId, uint postId, string text, CancellationToken cancellationToken);
        Task<int> DeletePostAsync(uint userId, uint postId, CancellationToken cancellationToken);
        Task<Post> GetPostAsync(uint userId, uint postId, CancellationToken cancellationToken);
        Task<IEnumerable<Post>> FeedPostsAsync(uint userId, uint offset, uint limit, CancellationToken cancellationToken);
    }
}
