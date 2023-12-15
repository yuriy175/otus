using Bff.API.Dtos;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;

namespace Bff.Infrastructure.gRpc.Services.Interfaces
{
    public interface IPostService
    {
        Task CreatePostAsync(uint userId, string text, CancellationToken cancellationToken);
        Task<IEnumerable<PostDto>> FeedPostsAsync(uint userId, uint offset, uint limit, CancellationToken cancellationToken);
    }
}