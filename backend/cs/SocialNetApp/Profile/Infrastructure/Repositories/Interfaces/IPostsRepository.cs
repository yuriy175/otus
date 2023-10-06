﻿using Dapper;
using SocialNetApp.Core.Model;

namespace Profile.Infrastructure.Repositories.Interfaces
{
    public interface IPostsRepository
    {
        Task<int> AddPostAsync(uint userId, string text, CancellationToken cancellationToken);
        Task<int> UpdatePostAsync(uint userId, uint postId, string text, CancellationToken cancellationToken);
        Task<int> DeletePostAsync(uint userId, uint postId, CancellationToken cancellationToken);
        Task<Post> GetPostAsync(uint userId, uint postId, CancellationToken cancellationToken);
        Task<IEnumerable<Post>> GetPostsAsync(uint userId, uint offset, uint limit, CancellationToken cancellationToken);
    }
}
