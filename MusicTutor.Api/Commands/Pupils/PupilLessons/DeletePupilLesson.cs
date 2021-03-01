using System;
using MediatR;
using MusicTutor.Api.Contracts.Lessons;

namespace MusicTutor.Api.Commands.Pupils
{
    public record DeletePupilLesson(Guid PupilId, Guid LessonId) : IRequest<int> {}     

}