using Auths;
using AutoMapper;
using Bff.API.Dtos;
using Bff.Infrastructure.gRpc.Services.Interfaces;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profile;

namespace Bff.API.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UserController(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("/login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoggedinUserDto))]
        public async Task<ActionResult<LoggedinUserDto>> Login([FromBody] LoginDto dto, CancellationToken cancellationToken)
        {
            return Ok(await _userService.LoginAsync(dto, cancellationToken));
        }
    }
}
