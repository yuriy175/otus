using AutoMapper;
using Bff.API.Dtos;
using Bff.Infrastructure.gRpc.Services.Interfaces;
using Common.API.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bff.API.Controllers
{
    [ApiController]
    public class FriendsController : AuthorizedControllerBase
    {
        private readonly IFriendService _friendService;

        public FriendsController(IFriendService friendService)
        {
            _friendService = friendService;
        }

        [Authorize]
        [HttpGet("/friends")]
        public async Task<ActionResult<IEnumerable<UserDto>?>> GetFriends(CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            if (userId is null)
            {
                return Unauthorized();
            }
            return Ok(await _friendService.GetFriendsAsync(userId.Value, cancellationToken));
        }

        [Authorize]
        [HttpPut("/friend/set/{user_id}")]
        public async Task<ActionResult<UserDto>> AddFriend([FromRoute(Name = "user_id")] uint friendId, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            if (userId is null || userId.Value == friendId)
            {
                return BadRequest();
            }
            return Ok(await _friendService.AddFriendAsync(userId.Value, friendId, cancellationToken));
        }

        [Authorize]
        [HttpDelete("/friend/delete/{user_id}")]
        public async Task<ActionResult> DeleteFriend([FromRoute(Name = "user_id")] uint friendId, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            if (userId is null)
            {
                return BadRequest();
            }
            await _friendService.DeleteFriendAsync(userId.Value, friendId, cancellationToken);
            return Ok();
        }
    }
}
