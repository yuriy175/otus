using AutoMapper;
using Bff.API.Dtos;
using Bff.Infrastructure.gRpc.Services.Interfaces;
using Common.API.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bff.API.Controllers
{
    [ApiController]
    public class DialogsController : AuthorizedControllerBase
    {
        private readonly IDialogService _dialogService;

        public DialogsController(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }

        [Authorize]
        [HttpGet("/buddies")]
        public async Task<ActionResult<IEnumerable<UserDto>?>> GetDialogBuddies(CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            if (userId is null)
            {
                return Unauthorized();
            }
            return Ok(await _dialogService.GetDialogBuddiesAsync(userId.Value, cancellationToken));
        }

        [HttpGet("/dialog/{user_id}/list")]
        public async Task<ActionResult<UserMessagesDto>> GetDialogByUserId([FromRoute(Name = "user_id")] uint userId, CancellationToken cancellationToken)
        {
            var authorId = GetUserId();
            if (authorId is null)
            {
                return Unauthorized();
            }

            return Ok(await _dialogService.GetMessagesAsync(authorId.Value, userId, cancellationToken));
        }

        [HttpPost("/dialog/{user_id}/send")]
        public async Task<ActionResult<MessageDto>> SendMessageToUser([FromRoute(Name = "user_id")] uint userId, [FromBody] string text, CancellationToken cancellationToken)
        {
            var authorId = GetUserId();
            if (authorId is null)
            {
                return Unauthorized();
            }

            return Ok(await _dialogService.CreateMessageAsync(authorId.Value, userId, text, cancellationToken));
        }
    }
}
