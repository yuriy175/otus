using Auths;
using AutoMapper;
using Grpc.Core;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Profile.Core.Model.Interfaces;
using System.Threading;
using static Auths.Auth;

namespace Profile.Infrastructure.gRpc.Services
{
    public class AuthService : AuthBase
    {
        private readonly IAuthService _authService;

        public AuthService(IMapper mapper, IAuthService authService)
        {
            _authService = authService;
        }

        public override async Task<LoginReply> Login(LoginRequest request, ServerCallContext context)
        {
            var token = await _authService.LoginAsync(request.Id, request.Password, context.CancellationToken);
            return new LoginReply { Token = token };
        }
    }
}
