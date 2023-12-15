using AutoMapper;
using Bff.API.Dtos;
using Bff.Infrastructure.gRpc.Services.Interfaces;
using Common.API.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace Bff.API.Controllers
{
    [ApiController]
    public class PostsController : AuthorizedControllerBase
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        [Authorize]
        [HttpPost("/post/create")]
        public async Task<ActionResult> CreatePost([FromBody] string text, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            if (userId is null || string.IsNullOrEmpty(text))
            {
                return BadRequest();
            }
            await _postService.CreatePostAsync(userId.Value, text, cancellationToken);
            return Ok();
        }

        [Authorize]
        [HttpGet("/post/feed")]
        public async Task<ActionResult<IEnumerable<PostDto>?>> FeedPosts([FromQuery] uint offset, [FromQuery] uint limit, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            if (userId is null)
            {
                return BadRequest();
            }
            return Ok(await _postService.FeedPostsAsync(userId.Value, offset, limit, cancellationToken));
        }
    }
}
