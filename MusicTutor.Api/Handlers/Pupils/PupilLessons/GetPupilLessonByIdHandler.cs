using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MusicTutor.Api.Contracts.Lessons;
using MusicTutor.Api.Queries.Pupils;
using MusicTutor.Core.Services;

namespace MusicTutor.Api.EFCore.Handlers.Pupils
{
    public record GetPupilLessonByIdHandler(IMusicTutorDbContext DbContext, IMapper Mapper) : IRequestHandler<GetPupilLessonById, LessonResponseDto>
    {
        public async Task<LessonResponseDto> Handle(GetPupilLessonById request, CancellationToken cancellationToken)
        {
            var pupil = await DbContext.Pupils.Where(p => p.Id == request.pupilId).Include(p => p.Lessons).SingleOrDefaultAsync();

            if (pupil is null)
                return null;

            var lesson = pupil.Lessons.SingleOrDefault(l => l.Id == request.lessonId);

            if (lesson is null)
                return null;

            return Mapper.Map<LessonResponseDto>(lesson);
        }
    }
}
