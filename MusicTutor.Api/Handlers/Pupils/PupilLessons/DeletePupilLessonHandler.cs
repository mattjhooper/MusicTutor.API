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
    public record DeletePupilLessonHandler(IMusicTutorDbContext DbContext) : IRequestHandler<DeletePupilLesson, int>
    {        
        public async Task<int> Handle(DeletePupilLesson request, CancellationToken cancellationToken)
        {
            var pupil = await DbContext.Pupils.Include(p => p.Lessons).SingleOrDefaultAsync(p => p.Id == request.PupilId);
            if (pupil is null)                
                return -1;

            var lesson = pupil.Lessons.SingleOrDefault(l => l.Id == request.LessonId);

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