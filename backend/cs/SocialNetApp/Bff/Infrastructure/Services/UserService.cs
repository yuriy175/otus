using Auths;
using Bff.API.Dtos;
using Bff.Infrastructure.gRpc.Services.Interfaces;
using Grpc.Net.Client;

namespace Profile.Infrastructure.gRpc.Services
{
    public class UserService : IUserService
    {
        private readonly static string _grpcUrl = Environment.GetEnvironmentVariable("GRPC_PROFILE");

        public UserService()
        {
        }

        public async Task<LoggedinUserDto> LoginAsync(LoginDto dto, CancellationToken cancellationToken)
        {
            var options = new GrpcChannelOptions();
            using var channel = GrpcChannel.ForAddress(_grpcUrl, options);
            var client = new Users.UsersClient(channel);
            var auth = new Auth.AuthClient(channel);

            var token = await auth.LoginAsync(new LoginRequest { Id = dto.Id, Password = dto.Password });
            var user = await client.GetUserByIdAsync(new GetUserByIdRequest { Id = dto.Id });
            var userDto = new UserDto
            {
                City = user.City,
                Id = user.Id,
                Info = user.Info,
                Name = user.Name,
                Sex = user.Sex,
                Surname = user.Surname,
                Age = user.Age.HasValue ? (byte)user.Age.Value : null as byte?,
            };

            return new LoggedinUserDto { User = userDto,
                Token = token.Token,
            };
        }
    }
}
