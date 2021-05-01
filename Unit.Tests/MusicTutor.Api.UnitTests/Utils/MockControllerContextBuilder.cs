using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MusicTutor.Api.UnitTests.Utils
{
    public static class MockControllerContextBuilder
    {
        public static ControllerContext GetControllerContext()
        {
            var claim = new Claim("Id", Guid.NewGuid().ToString());
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(claim);
            var claims = new List<ClaimsIdentity>();
            claims.Add(claimsIdentity);
            var controllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new System.Security.Claims.ClaimsPrincipal(claims)
                }
            };

            return controllerContext;
        }
    }
}