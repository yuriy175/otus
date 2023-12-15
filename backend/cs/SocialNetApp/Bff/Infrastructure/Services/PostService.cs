using Auths;
using AutoMapper;
using Bff.API.Dtos;
using Bff.Infrastructure.gRpc.Services.Interfaces;
using FriendGrpc;
using Grpc.Net.Client;
using ProfileGrpc;
using RabbitMQ.Client;
using System.Diagnostics;
using static FriendGrpc.Friend;
using static ProfileGrpc.Users;

namespace Profile.Infrastructure.gRpc.Services
{
    public class PostService : IPostService
    {
        private readonly static string _grpcPostsUrl = Environment.GetEnvironmentVariable("GRPC_POSTS");
        private readonly static string _grpcUsersUrl = Environment.GetEnvironmentVariable("GRPC_PROFILE");

        private readonly IMapper _mapper;

        private static readonly GrpcChannel _usersChannel = null;
        private static readonly GrpcChannel _postsChannel = null;

        static PostService()
        {
            var options = new GrpcChannelOptions()
            {
                HttpHandler = new SocketsHttpHandler
                {
                    EnableMultipleHttp2Connections = true,
                }
            };
            _postsChannel = GrpcChannel.ForAddress(_grpcPostsUrl, options);
            _usersChannel = GrpcChannel.ForAddress(_grpcUsersUrl, options);

            _postsChannel.ConnectAsync().Wait();
            _usersChannel.ConnectAsync().Wait();
        }

        public static Task WarmupChannels(){ return Task.CompletedTask; }

        public FriendService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<UserDto> AddFriendAsync(uint userId, uint friendId, CancellationToken cancellationToken)
        {
            var options = new GrpcChannelOptions();
            //using var postsChannel = GrpcChannel.ForAddress(_grpcPostsUrl, options);
            //using var usersChannel = GrpcChannel.ForAddress(_grpcUsersUrl, options);
            var userClient = new Users.UsersClient(_usersChannel);
            var friendClient = new FriendClient(_postsChannel);
            await friendClient.AddFriendAsync(new AddFriendRequest { UserId = userId, FriendId = friendId });

            var user = await userClient.GetUserByIdAsync(new GetUserByIdRequest { Id = friendId });
            return new UserDto
            {
                City = user.City,
                Id = user.Id,
                Info = user.Info,
                Name = user.Name,
                Sex = user.Sex,
                Surname = user.Surname,
                Age = user.Age.HasValue ? (byte)user.Age.Value : null as byte?,
            };
        }

        public async Task DeleteFriendAsync(uint userId, uint friendId, CancellationToken cancellationToken)
        {
            var friendClient = new FriendClient(_postsChannel);

            await friendClient.DeleteFriendAsync(new DeleteFriendRequest { UserId = userId, FriendId = friendId });
        }

        public async Task<IEnumerable<UserDto>> GetFriendsAsync(uint userId, CancellationToken cancellationToken)
        {
            var st = Stopwatch.StartNew();            
            var userClient = new UsersClient(_usersChannel);
            var friendClient = new FriendClient(_postsChannel);

            var list = new List<long> { };
            list.Add(st.ElapsedMilliseconds);
            var friendIds = await friendClient.GetFriendIdsAsync(new GetFriendIdsRequest { Id = userId});
            list.Add(st.ElapsedMilliseconds);
            var friends = new List<UserDto>();
            foreach (var friendId in friendIds.Ids)
            {
                var user = await userClient.GetUserByIdAsync(new GetUserByIdRequest { Id = friendId });
                list.Add(st.ElapsedMilliseconds);
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
            list.Add(st.ElapsedMilliseconds);
            return friends;
        }
    }
}
