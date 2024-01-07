using Auths;
using AutoMapper;
using Bff.API.Dtos;
using Bff.Infrastructure.gRpc.Services.Interfaces;
using Bff.Infrastructure.Services.Interfaces;
using FriendGrpc;
using Grpc.Net.Client;
using ProfileGrpc;
using RabbitMQ.Client;
using System.Diagnostics;
using static FriendGrpc.Friend;
using static ProfileGrpc.Users;

namespace Bff.Infrastructure.gRpc.Services
{
    public class FriendService : IFriendService
    {
        private readonly IMapper _mapper;
        private readonly IGrpcChannelsProvider _channelsProvider;

        public FriendService(IMapper mapper, IGrpcChannelsProvider channelsProvider)
        {
            _mapper = mapper;
            _channelsProvider = channelsProvider;
        }

        public async Task<UserDto> AddFriendAsync(uint userId, uint friendId, CancellationToken cancellationToken)
        {
            var userClient = new UsersClient(_channelsProvider.GetUsersChannel());
            var friendClient = new FriendClient(_channelsProvider.GetPostsChannel());
            await friendClient.AddFriendAsync(new AddFriendRequest { UserId = userId, FriendId = friendId });

            var user = await userClient.GetUserByIdAsync(new GetUserByIdRequest { Id = friendId });
            return _mapper.Map<UserDto>(user);
        }

        public async Task DeleteFriendAsync(uint userId, uint friendId, CancellationToken cancellationToken)
        {
            var friendClient = new FriendClient(_channelsProvider.GetPostsChannel());

            await friendClient.DeleteFriendAsync(new DeleteFriendRequest { UserId = userId, FriendId = friendId });
        }

        public async Task<IEnumerable<UserDto>> GetFriendsAsync(uint userId, CancellationToken cancellationToken)
        {
            var st = Stopwatch.StartNew();            
            var userClient = new Users.UsersClient(_channelsProvider.GetUsersChannel());
            var friendClient = new FriendClient(_channelsProvider.GetPostsChannel());

            var list = new List<long> { };
            list.Add(st.ElapsedMilliseconds);
            var friendIds = await friendClient.GetFriendIdsAsync(new GetFriendIdsRequest { Id = userId});
            list.Add(st.ElapsedMilliseconds);
            var friends = new List<UserDto>();
            foreach (var friendId in friendIds.Ids)
            {
                var user = await userClient.GetUserByIdAsync(new GetUserByIdRequest { Id = friendId });
                list.Add(st.ElapsedMilliseconds);
                friends.Add(_mapper.Map<UserDto>(user));
            }
            list.Add(st.ElapsedMilliseconds);
            return friends;
        }

        //private UserDto ToUserDto(UserReply user) => new UserDto
        //{
        //    City = user.City,
        //    Id = user.Id,
        //    Info = user.Info,
        //    Name = user.Name,
        //    Sex = user.Sex,
        //    Surname = user.Surname,
        //    Age = user.Age.HasValue ? (byte)user.Age.Value : null as byte?,
        //};
    }
}
