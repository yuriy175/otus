using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Profile.Core.Model.Interfaces;
using SocialNetApp.API.Dtos;

namespace SocialNetApp.API.Controllers
{
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IAuthService _authService;

        public PostsController(IAuthService authService)
        {
            _authService = authService;
        }

        //[HttpPost("/login")]
        //public async Task<ActionResult> Login([FromBody]LoginDto dto, CancellationToken cancellationToken)
        //{
        //    return Ok(await _authService.LoginAsync(dto.Id, dto.Password));
        //}
    }
}
