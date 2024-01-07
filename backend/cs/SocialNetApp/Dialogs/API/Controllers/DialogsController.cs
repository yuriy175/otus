using AutoMapper;
using Common.API.Controllers;
using Dialogs.Core.Model;
using Dialogs.Core.Model.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using static System.Net.Mime.MediaTypeNames;

namespace Dialogs.API.Controllers
{
    [ApiController]
    public class DialogsController : AuthorizedControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IDialogsService _dialogsService;

        public DialogsController(IMapper mapper, IDialogsService dialogsService)
        {
            _mapper = mapper;
            _dialogsService = dialogsService;
        }

        [HttpGet("/dialog/{user_id}/list")]
        public async Task<ActionResult<IEnumerable<Message>>> GetDialogByUserId([FromRoute(Name = "user_id")] uint userId, CancellationToken cancellationToken)
        {
            var authorId = GetUserId();
            if (authorId is null)
            {
                return Unauthorized();
            }

            return Ok(await _dialogsService.GetMessagesAsync(authorId.Value, userId, cancellationToken));
        }

        [HttpPost("/dialog/{user_id}/send")]
        public async Task<ActionResult<Message>> SendMessageToUser([FromRoute(Name = "user_id")] uint userId, [FromBody] string text, CancellationToken cancellationToken)
        {
            var authorId = GetUserId();
            if (authorId is null)
            {
                return Unauthorized();
            }

            return Ok(await _dialogsService.CreateMessageAsync(authorId.Value, userId, text, cancellationToken));
        }

        [Authorize]
        [HttpGet("/buddies")]
        public async Task<ActionResult<IEnumerable<int>?>> GetBuddies(CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            if (userId is null)
            {
                return BadRequest();
            }
            return Ok(await _dialogsService.GetBuddyIdsAsync(userId.Value, cancellationToken));
        }
    }
}
