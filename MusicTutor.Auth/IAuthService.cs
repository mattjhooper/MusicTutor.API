using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MusicTutor.Services.Auth
{
    public interface IAuthService
    {
        Task<IdentityUser> FindIdentityUserByEmailAsync(string email);

        Task<bool> CheckPasswordAsync(IdentityUser existingUser, string password);

        Task<IdentityResult> CreateIdentityUserAsync(IdentityUser newUser, string password);

        string GenerateJwtToken(IdentityUser user);
    }
}