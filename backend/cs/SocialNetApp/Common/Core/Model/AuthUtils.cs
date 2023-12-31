﻿using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Common.Core.Model
{
    public static  class AuthUtils
    {
        private readonly static string? _securityKey = Environment.GetEnvironmentVariable("SECURITY_KEY");

        static AuthUtils()
        {
            if (_securityKey is null)
            {
                throw new ApplicationException("Пустой секьюрити ключ");
            }
        }

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_securityKey!));
        }

        public static uint? GetAuthorizedUserId(string tokenString)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(tokenString);

            var userId = jwtSecurityToken.Claims.First(claim => claim.Type == Constants.UserIdClaimType).Value;

            return string.IsNullOrEmpty(userId) ? null : Convert.ToUInt32(userId);
        }
    }
}
