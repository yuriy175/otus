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
    public class FriendsService : IFriendsService
    {        
        private readonly IFriendsRepository _friendsRepository;

        public FriendsService(IFriendsRepository friendsRepository)
        {
            _friendsRepository = friendsRepository;
        }

        public Task<int> UpsertFriendAsync(uint userId, uint friendId, CancellationToken cancellationToken) =>
            _friendsRepository.UpsertFriendAsync(userId, friendId, cancellationToken);

        public Task<int> DeleteFriendAsync(uint userId, uint friendId, CancellationToken cancellationToken) =>
            _friendsRepository.DeleteFriendAsync(userId, friendId, cancellationToken);
    }
}
