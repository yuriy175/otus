using Common.API.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Posts.Application.Commands.Friends;
using Posts.Application.Queries.Friends;
using Posts.Core.Model.Interfaces;

namespace Posts.API.Controllers
{
    [ApiController]
    public class FriendsController : AuthorizedControllerBase
    {
        private readonly IMediator _mediator;

        public FriendsController(IMediator mediator)
        {
            _mediator = mediator;
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
            return Ok(await _mediator.Send(new AddFriendCommand { UserId = userId.Value, FriendId = friendId }));
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
            return Ok(await _mediator.Send(new DeleteFriendCommand { UserId = userId.Value, FriendId = friendId }));
        }

        [Authorize]
        [HttpGet("/friends")]
        public async Task<ActionResult<IEnumerable<int>?>> GetFriends(CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            if (userId is null)
            {
                return BadRequest();
            }
            var friends = await _mediator.Send(new GetUserFriendsQuery { UserId = userId.Value });
            return Ok(friends);
        }
    }
}
