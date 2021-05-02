using MediatR;
using MusicTutor.Api.Contracts.Auth;

namespace MusicTutor.Api.Commands.Auth
{
    public record RegisterUser(string Name, string Email, string Password) : IRequest<RegistrationResponseDto> { }

}