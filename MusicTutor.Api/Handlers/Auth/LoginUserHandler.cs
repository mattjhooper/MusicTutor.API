using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MusicTutor.Api.Commands.Auth;
using MusicTutor.Api.Contracts.Auth;
using MusicTutor.Api.Settings;
using MusicTutor.Services.Auth;

namespace MusicTutor.Api.EFCore.Handlers.Auth
{
    public class LoginUserHandler : IRequestHandler<LoginUser, RegistrationResponseDto>
    {
        private readonly IAuthService _authService;
        public LoginUserHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<RegistrationResponseDto> Handle(LoginUser user, CancellationToken cancellationToken)
        {

            // check if the user with the same email exist
            var existingUser = await _authService.FindUserByEmailAsync(user.Email);

            if (existingUser == null)
            {
                // We dont want to give to much information on why the request has failed for security reasons
                return new RegistrationResponseDto()
                {
                    Result = false,
                    Errors = new List<string>(){
                                        "Invalid authentication request"
                                    }
                };
            }

            // Now we need to check if the user has inputed the right password
            var isCorrect = await _authService.CheckPasswordAsync(existingUser, user.Password);

            if (isCorrect)
            {
                var jwtToken = _authService.GenerateJwtToken(existingUser);

                return new RegistrationResponseDto()
                {
                    Result = true,
                    Token = jwtToken
                };
            }
            else
            {
                // We dont want to give to much information on why the request has failed for security reasons
                return new RegistrationResponseDto()
                {
                    Result = false,
                    Errors = new List<string>(){
                                         "Invalid authentication request"
                                    }
                };
            }


        }
    }
}