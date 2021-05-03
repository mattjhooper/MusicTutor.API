using System;
using MediatR;
using MusicTutor.Api.Contracts.Pupils;
using MusicTutor.Core.Models;

namespace MusicTutor.Api.Commands.Pupils
{
    public record MusicTutorUserCreatePupil(Guid MusicTutorUserId, CreatePupil CreatePupil) : IRequest<PupilResponseDto>
    {
    }

}