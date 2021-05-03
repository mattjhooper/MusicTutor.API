using System;
using System.Threading;
using System.Threading.Tasks;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MusicTutor.Api.Commands.Pupils;
using MusicTutor.Core.Services;
using MusicTutor.Api.Contracts.Lessons;
using System.Linq;

namespace MusicTutor.Api.EFCore.Handlers.Pupils
{
    public record DeletePupilLessonHandler(IMusicTutorDbContext DbContext) : IRequestHandler<WithMusicTutorUserId<DeletePupilLesson, int>, int>
    {
        public async Task<int> Handle(WithMusicTutorUserId<DeletePupilLesson, int> request, CancellationToken cancellationToken)
        {
            var deletePupilLesson = request.Request;
            var pupil = await DbContext.GetPupilWithLessonsForUserAsync(deletePupilLesson.PupilId, request.MusicTutorUserId);
            if (pupil is null)
                return -1;

            var lesson = pupil.Lessons.SingleOrDefault(l => l.Id == deletePupilLesson.LessonId);

            if (lesson is null)
                return 0;

            var isLessonDeleted = pupil.RemoveLesson(lesson);

            if (isLessonDeleted)
            {
                await DbContext.SaveChangesAsync(cancellationToken);
                return 1;
            }

            return 0;
        }
    }
}