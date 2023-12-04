using Auths;
using Bff.API.Dtos;
using Bff.Infrastructure.gRpc.Services.Interfaces;
using Grpc.Net.Client;
using static Friend.Friend;

namespace Profile.Infrastructure.gRpc.Services
{
    public class FriendService : IFriendService
    {
        private readonly static string _grpcPostsUrl = Environment.GetEnvironmentVariable("GRPC_POSTS");
        private readonly static string _grpcUsersUrl = Environment.GetEnvironmentVariable("GRPC_PROFILE");

        public FriendService()
        {
        }

        public async Task<IEnumerable<UserDto>> GetFriendsAsync(uint userId, CancellationToken cancellationToken)
        {
            var options = new GrpcChannelOptions();
            using var postsChannel = GrpcChannel.ForAddress(_grpcPostsUrl, options);
            using var usersChannel = GrpcChannel.ForAddress(_grpcUsersUrl, options);
            var userClient = new Users.UsersClient(usersChannel);
            var friendClient = new FriendClient(postsChannel);

            var friendIds = await friendClient.GetFriendIdsAsync(new Friend.GetFriendIdsRequest { Id = userId});
            var friends = new List<UserDto>();
            foreach (var friendId in friendIds.Ids)
            {
                var user = await userClient.GetUserByIdAsync(new GetUserByIdRequest { Id = friendId });
                friends.Add(new UserDto
                {
                    City = user.City,
                    Id = user.Id,
                    Info = user.Info,
                    Name = user.Name,
                    Sex = user.Sex,
                    Surname = user.Surname,
                    Age = user.Age.HasValue ? (byte)user.Age.Value : null as byte?,
                });
            }

            return friends;
        }
    }
}
