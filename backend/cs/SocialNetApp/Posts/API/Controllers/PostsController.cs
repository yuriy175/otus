using Common.API.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Posts.API.Dtos;
using Posts.Application.Commands.Posts;
using Posts.Application.Queries.Posts;

namespace Posts.API.Controllers
{
    [ApiController]
    public class PostsController : AuthorizedControllerBase
    {
        private IMediator _mediator;

        public PostsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpPost("/post/create")]
        public async Task<ActionResult> CreatePost([FromBody]string text, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            if (userId is null || string.IsNullOrEmpty(text))
            {
                return BadRequest();
            }
            return Ok(await _mediator.Send(new AddPostCommand { UserId = userId.Value, Text = text }));
        }

        [Authorize]
        [HttpPut("/post/update")]
        public async Task<ActionResult> UpdatePost([FromBody] UpdatePostDto post, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            if (userId is null || string.IsNullOrEmpty(post.Text))
            {
                return BadRequest();
            }
            return Ok(await _mediator.Send(new UpdatePostCommand { UserId = userId.Value, Text = post.Text, PostId = post.PostId }));
        }

        [Authorize]
        [HttpDelete("/post/delete/{id}")]
        public async Task<ActionResult> DeletePost([FromRoute(Name = "id")] uint postId, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            if (userId is null)
            {
                return BadRequest();
            }
            return Ok(await _mediator.Send(new DeletePostCommand { UserId = userId.Value, PostId = postId }));
        }

        [AllowAnonymous]
        [HttpGet("/post/get/{id}")]
        public async Task<ActionResult> GetPost([FromRoute(Name = "id")] uint postId, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            if (userId is null)
            {
                return BadRequest();
            }
            return Ok(await _mediator.Send(new GetUserPostQuery { UserId = userId.Value, PostId = postId }));
        }

        [Authorize]
        [HttpGet("/post/feed")]
        public async Task<ActionResult> FeedPosts([FromQuery]uint offset, [FromQuery] uint limit, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            if (userId is null)
            {
                return BadRequest();
            }
            return Ok(await _mediator.Send(new FeedPostsQuery { UserId = userId.Value, Offset = offset, Limit = limit }));
        }
    }
}
