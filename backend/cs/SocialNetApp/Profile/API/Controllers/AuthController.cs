using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Profile.Core.Model.Interfaces;
using SocialNetApp.API.Dtos;
using System.Net.Mime;

namespace SocialNetApp.API.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("/login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type=typeof(LoginResultDto))]
        //[Consumes(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<LoginResultDto>> Login([FromBody]LoginDto dto, CancellationToken cancellationToken)
        {
            return Ok(new LoginResultDto { Token = await _authService.LoginAsync(dto.Id, dto.Password, cancellationToken) });
        }
    }
}
