using System;
using MediatR;
using MusicTutor.Api.Contracts.Pupils;
using MusicTutor.Core.Models;

namespace MusicTutor.Api.Commands.Auth
{
    public record WithMusicTutorUserId<TRequest, TResponse>(Guid MusicTutorUserId, TRequest Request) : IRequest<TResponse>
    {
    }

}