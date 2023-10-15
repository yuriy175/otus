using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Profile.Core.Model;
using Profile.Core.Model.Interfaces;
using SocialNetApp.API.Dtos;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SocialNetApp.API.Controllers
{
    public class AuthorizedControllerBase : ControllerBase
    {
        public AuthorizedControllerBase()
        {
        }

        protected uint? GetUserId()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var claimValue = identity?.Claims?.FirstOrDefault(c => c.Type == Constants.UserIdClaimType)?.Value;

            return string.IsNullOrEmpty(claimValue)? null : Convert.ToUInt32(claimValue);
        }
    }
}
