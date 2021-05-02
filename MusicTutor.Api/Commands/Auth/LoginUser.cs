using MediatR;
using MusicTutor.Api.Contracts.Auth;

namespace MusicTutor.Api.Commands.Auth
{
    public record LoginUser(string Email, string Password) : IRequest<RegistrationResponseDto> { }

}