using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MusicTutor.Services.Auth
{
    public interface IAuthService
    {
        Task<MusicTutorUser> FindUserByEmailAsync(string email);

        Task<bool> CheckPasswordAsync(MusicTutorUser existingUser, string password);

        Task<IdentityResult> CreateUserAsync(MusicTutorUser newUser, string password);

        string GenerateJwtToken(MusicTutorUser user);
    }
}