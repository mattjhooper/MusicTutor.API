using System;
using System.Collections.Generic;
using MediatR;
using MusicTutor.Api.Contracts.Lessons;

namespace MusicTutor.Api.Queries.Pupils
{
    public record GetPupilLessonById(Guid PupilId, Guid LessonId) : IRequest<LessonResponseDto> { }

}