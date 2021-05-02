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
    public class RegisterUserHandler : IRequestHandler<RegisterUser, RegistrationResponseDto>
    {
        private readonly IAuthService _authService;

        public RegisterUserHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<RegistrationResponseDto> Handle(RegisterUser user, CancellationToken cancellationToken)
        {

            // check if the user with the same email already exists
            var existingUser = await _authService.FindUserByEmailAsync(user.Email);

            if (existingUser != null)
            {
                return new RegistrationResponseDto()
                {
                    Result = false,
                    Errors = new List<string>(){
                                        "Email already exists"
                                    }
                };
            }

            var newUser = new MusicTutorUser() { Email = user.Email, UserName = user.Email };
            var isCreated = await _authService.CreateUserAsync(newUser, user.Password);
            if (isCreated.Succeeded)
            {
                var jwtToken = _authService.GenerateJwtToken(newUser);

                return new RegistrationResponseDto()
                {
                    Result = true,
                    Token = jwtToken
                };
            }

            return new RegistrationResponseDto()
            {
                Result = false,
                Errors = isCreated.Errors.Select(x => x.Description).ToList()
            };
        }
    }
}