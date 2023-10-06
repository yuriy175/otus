using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.IdentityModel.Tokens;
using Profile.Core.Model;
using Profile.Core.Model.Interfaces;
using Profile.Infrastructure.Repositories;
using Profile.Infrastructure.Repositories.Interfaces;
using SocialNetApp.Core.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Profile.Core.Services
{
    public class PostsService : IPostsService
    {        
        private readonly IPostsRepository _postsRepository;

        public PostsService(IPostsRepository postsRepository)
        {
            _postsRepository = postsRepository;
        }

        public Task<int> CreatePostAsync(uint userId, string text, CancellationToken cancellationToken) =>
            _postsRepository.AddPostAsync(userId, text, cancellationToken);

        public Task<int> UpdatePostAsync(uint userId, uint postId, string text, CancellationToken cancellationToken) =>
            _postsRepository.UpdatePostAsync(userId, postId, text, cancellationToken);

        public Task<int> DeletePostAsync(uint userId, uint postId, CancellationToken cancellationToken) =>
            _postsRepository.DeletePostAsync(userId, postId, cancellationToken);

        public Task<Post> GetPostAsync(uint userId, uint postId, CancellationToken cancellationToken) =>
            _postsRepository.GetPostAsync(userId, postId, cancellationToken);

        public Task<IEnumerable<Post>> FeedPostsAsync(uint userId, uint offset, uint limit, CancellationToken cancellationToken) =>
            _postsRepository.GetPostsAsync(userId, offset, limit, cancellationToken);
    }
}
