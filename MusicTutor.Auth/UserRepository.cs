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
                ClaimsPrincipal principal = _httpContextAccessor?.HttpContext?.User as ClaimsPrincipal;
                var userId = Guid.Empty;

                var parsed = System.Guid.TryParse(principal?.Claims?.FirstOrDefault(c => c.Type == "Id")?.Value, out userId);

                if (parsed)
                    return userId;

                return Guid.Empty;
            }
        }
    }
}