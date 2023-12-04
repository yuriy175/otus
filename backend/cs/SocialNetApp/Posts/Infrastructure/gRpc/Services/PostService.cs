namespace Profile.Infrastructure.gRpc.Services
{
    //public class UserService : Users.UsersBase
    //{
    //    private readonly IMapper _mapper;
    //    private readonly IUserService _userService;

    //    public UserService(IMapper mapper, IUserService userService)
    //    {
    //        _mapper = mapper;
    //        _userService = userService;
    //    }

    //    public override async Task<GetUserByIdReply> GetUserById(GetUserByIdRequest request, ServerCallContext context)
    //    {
    //        var user = await _userService.GetUserByIdAsync(request.Id);
    //        return new GetUserByIdReply { 
    //            City = user.City,
    //            Id = user.Id,
    //            Info = user.Info,
    //            Name = user.Name,
    //            Sex = user.Sex,
    //            Surname = user.Surname,
    //            Age = user.Age
    //        };
    //    }
    //}
}
