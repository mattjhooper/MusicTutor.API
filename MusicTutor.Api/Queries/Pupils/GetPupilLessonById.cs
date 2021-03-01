using System;
using System.Collections.Generic;
using MediatR;
using MusicTutor.Api.Contracts.Lessons;

namespace MusicTutor.Api.Queries.Pupils
{
    public record GetPupilLessonById(Guid pupilId, Guid lessonId) : IRequest<LessonResponseDto> {}     

}