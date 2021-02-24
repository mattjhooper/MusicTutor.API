using System;
using MediatR;
using MusicTutor.Api.Contracts.Lessons;

namespace MusicTutor.Api.Commands.Pupils
{
    public record CreatePupilLesson(Guid PupilId, DateTime StartDateTime, int DurationInMinutes, decimal Cost, bool IsPlanned) : IRequest<LessonResponseDto> {}     

}