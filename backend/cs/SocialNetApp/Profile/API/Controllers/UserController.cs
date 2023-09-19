using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Profile.Core.Model.Interfaces;
using Profile.Infrastructure.Repositories.Interfaces;
using SocialNetApp.API.Dtos;
using SocialNetApp.Core.Model;
using System;

namespace SocialNetApp.API.Controllers
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

        [HttpGet("/users")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers(CancellationToken cancellationToken)
        {
            return Ok(await _userService.GetUsersAsync());
        }

        [HttpGet("/user/get/{id}")]
        public async Task<ActionResult<User>> GetUserById([FromRoute] uint id, CancellationToken cancellationToken)
        {
            return Ok(await _userService.GetUserByIdAsync(id));
        }

        [HttpPost("/user/register")]
        public async Task<ActionResult> RegisterUser([FromBody] NewUserDto dto, CancellationToken cancellationToken)
        {
            return Ok(new {id = await _userService.PutUserAsync(_mapper.Map<User>(dto), dto.Password)} );
        }

        [HttpGet("/user/search")]
        public async Task<ActionResult> FindUsers([FromQuery] SearchUserDto dto, CancellationToken cancellationToken)
        {
            return Ok(await _userService.GetUsersByNameAsync(dto.Name, dto.Surname));
        }
    }
}
