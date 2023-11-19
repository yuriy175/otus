using Common.API.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Posts.Core.Model.Interfaces;

namespace Posts.API.Controllers
{
    [ApiController]
    public class FriendsController : AuthorizedControllerBase
    {
        private readonly IFriendsService _friendsService;

        public FriendsController(IFriendsService friendsService)
        {
            _friendsService = friendsService;
        }

        [Authorize]
        [HttpPut("/friend/set/{user_id}")]
        public async Task<ActionResult> AddFriend([FromRoute(Name = "user_id")]uint friendId, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            if(userId is null || userId.Value == friendId)
            {
                return BadRequest();
            }
            return Ok( await _friendsService.UpsertFriendAsync(userId.Value, friendId, cancellationToken));
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
            return Ok(await _friendsService.DeleteFriendAsync(userId.Value, friendId, cancellationToken));
        }
    }
}
