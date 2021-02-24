using System;
using System.Threading;
using System.Threading.Tasks;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MusicTutor.Api.Commands.Pupils;
using MusicTutor.Core.Services;
using MusicTutor.Api.Contracts.Lessons;
using MusicTutor.Core.Models;

namespace MusicTutor.Api.EFCore.Handlers.Pupils
{
    public record CreatePupilLessonHandler(IMusicTutorDbContext DbContext, IMapper Mapper) : IRequestHandler<CreatePupilLesson, LessonResponseDto>
    {        
        public async Task<LessonResponseDto> Handle(CreatePupilLesson createPupilLesson, CancellationToken cancellationToken)
        {
            var pupil = await DbContext.Pupils.SingleOrDefaultAsync(p => p.Id == createPupilLesson.PupilId);
            if (pupil is null)                
                return null;

            var lesson = new Lesson(createPupilLesson.StartDateTime, createPupilLesson.DurationInMinutes, createPupilLesson.Cost, createPupilLesson.IsPlanned);
            pupil.AddLesson(lesson) ;  

            await DbContext.SaveChangesAsync(cancellationToken);

            return Mapper.Map<LessonResponseDto>(lesson);
        }
    }
}