using Common.API.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Posts.API.Dtos;
using Posts.Core.Model.Interfaces;

namespace Posts.API.Controllers
{
    [ApiController]
    public class PostsController : AuthorizedControllerBase
    {
        private readonly IPostsService _friendsService;

        public PostsController(IPostsService friendsService)
        {
            _friendsService = friendsService;
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
            return Ok(await _friendsService.CreatePostAsync(userId.Value, text, cancellationToken));
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
            return Ok(await _friendsService.UpdatePostAsync(userId.Value, post.PostId, post.Text, cancellationToken));
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
            return Ok(await _friendsService.DeletePostAsync(userId.Value, postId, cancellationToken));
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
            return Ok(await _friendsService.GetPostAsync(userId.Value, postId, cancellationToken));
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
            return Ok(await _friendsService.FeedPostsAsync(userId.Value, offset, limit, cancellationToken));
        }
    }
}
