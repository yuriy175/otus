using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Security.Claims;
using Common.Core.Model;

namespace Common.API.Controllers
{
    public class AuthorizedControllerBase : ControllerBase
    {
        public AuthorizedControllerBase()
        {
        }

        protected uint? GetUserId()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var claimValue = identity?.Claims?.FirstOrDefault(c => c.Type == Core.Model.Constants.UserIdClaimType)?.Value;

            return string.IsNullOrEmpty(claimValue)? null : Convert.ToUInt32(claimValue);
        }
    }
}
