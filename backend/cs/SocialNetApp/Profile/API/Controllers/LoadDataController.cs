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
    [Route("[controller]")]
    public class LoadDataController : ControllerBase
    {
        private readonly ILoadDataService _loadDataService;

        public LoadDataController(ILoadDataService loadDataService)
        {
            _loadDataService = loadDataService;
        }

        [HttpPost("cities")]
        public async Task<ActionResult> LoadCities(CancellationToken cancellationToken)
        {            
            return Ok(await _loadDataService.LoadCitiesAsync());
        }

        [HttpPost("users")]
        public async Task<ActionResult> LoadUsers(CancellationToken cancellationToken)
        {            
            return Ok(await _loadDataService.LoadUsersAsync());
        }
    }
}
