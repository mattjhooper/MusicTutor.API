using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace MusicTutor.Services.Auth
{
    public class UserRepository : IUserRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserRepository(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public Guid UserId
        {
            get
            {
                ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
                var userId = System.Guid.Parse(principal.Claims.FirstOrDefault(c => c.Type == "Id")?.Value);

                return userId;
            }
        }
    }
}