using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MusicTutor.Api.Commands.Pupils;
using MusicTutor.Core.Services;
using System.Linq;
using MusicTutor.Api.Commands.Auth;

namespace MusicTutor.Api.EFCore.Handlers.Pupils
{
    public record DeletePupilLessonHandler(IMusicTutorDbContext DbContext) : IRequestHandler<DeletePupilLesson, int>
    {
        public async Task<int> Handle(DeletePupilLesson deletePupilLesson, CancellationToken cancellationToken)
        {
            var pupil = await DbContext.GetPupilWithLessonsAsync(deletePupilLesson.PupilId);
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