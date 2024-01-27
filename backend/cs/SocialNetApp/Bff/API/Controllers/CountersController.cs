using AutoMapper;
using Bff.API.Dtos;
using Bff.Infrastructure.gRpc.Services.Interfaces;
using Common.API.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bff.API.Controllers
{
    [ApiController]
    public class CountersController : AuthorizedControllerBase
    {
        private readonly ICounterService _counterService;

        public CountersController(ICounterService counterService)
        {
            _counterService = counterService;
        }

        [Authorize]
        [HttpGet("/unread/count")]
        public async Task<ActionResult<uint>> GetUnReadCounterByUserId(CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            if (userId is null)
            {
                return Unauthorized();
            }

            return Ok(await _counterService.GetUnReadCounterByUserIdAsync(userId.Value, cancellationToken));
        }
    }
}
