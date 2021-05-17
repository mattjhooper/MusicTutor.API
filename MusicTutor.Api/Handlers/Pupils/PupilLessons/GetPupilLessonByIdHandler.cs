using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MapsterMapper;
using MediatR;
using MusicTutor.Api.Contracts.Lessons;
using MusicTutor.Api.Queries.Pupils;
using MusicTutor.Core.Services;

namespace MusicTutor.Api.EFCore.Handlers.Pupils
{
    public record GetPupilLessonByIdHandler(IMusicTutorDbContext DbContext, IMapper Mapper) : IRequestHandler<GetPupilLessonById, LessonResponseDto>
    {
        public async Task<LessonResponseDto> Handle(GetPupilLessonById getPupilLessonById, CancellationToken cancellationToken)
        {
            var pupil = await DbContext.GetPupilWithLessonsAsync(getPupilLessonById.PupilId);

            if (pupil is null)
                return null;

            var lesson = pupil.Lessons.SingleOrDefault(l => l.Id == getPupilLessonById.LessonId);

            if (lesson is null)
                return null;

            return Mapper.Map<LessonResponseDto>(lesson);
        }
    }
}
