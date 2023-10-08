using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Profile.Core.Model.Interfaces;
using SocialNetApp.API.Dtos;

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
        public async Task<ActionResult> Login([FromBody]LoginDto dto, CancellationToken cancellationToken)
        {
            return Ok(await _authService.LoginAsync(dto.Id, dto.Password, cancellationToken));
        }
    }
}
