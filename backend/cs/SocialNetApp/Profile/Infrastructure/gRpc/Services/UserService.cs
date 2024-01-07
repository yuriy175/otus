using AutoMapper;
using Grpc.Core;
using Profile.Core.Model.Interfaces;
using ProfileGrpc;
using SocialNetApp.Core.Model;
using static ProfileGrpc.Users;

namespace Profile.Infrastructure.gRpc.Services
{
    public class UserService : UsersBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UserService(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        public override async Task<UserReply> GetUserById(GetUserByIdRequest request, ServerCallContext context)
        {
            var user = await _userService.GetUserByIdAsync(request.Id);
            return ToUserReply(user);
        }

        public override async Task<AddUserReply> AddUser(AddUserRequest request, ServerCallContext context)
        {
            var requestUser = request.User;
            var userId = await _userService.PutUserAsync(new User
            {
                City = requestUser.City,
                Info = requestUser.Info,
                Name = requestUser.Name,
                Sex = requestUser.Sex,
                Surname = requestUser.Surname,
            }, request.Password);

            return new AddUserReply { Id = Convert.ToUInt32(userId) };
        }

        public override async Task GetUsersByName(GetUsersByNameRequest request, IServerStreamWriter<UserReply> responseStream, ServerCallContext context)
        {
            var users = await _userService.GetUsersByNameAsync(request.Name, request.Surname);
            foreach (var user in users)
            {
                await responseStream.WriteAsync(ToUserReply(user));
            }
        }

        private UserReply ToUserReply(User user) => new UserReply
        {
            City = user.City,
            Id = user.Id,
            Info = user.Info,
            Name = user.Name,
            Sex = user.Sex,
            Surname = user.Surname,
            Age = user.Age
        };
    }
}
