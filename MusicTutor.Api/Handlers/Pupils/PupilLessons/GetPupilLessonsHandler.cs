using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MapsterMapper;
using MediatR;
using MusicTutor.Api.Commands.Auth;
using MusicTutor.Api.Contracts.Lessons;
using MusicTutor.Api.Queries.Pupils;
using MusicTutor.Core.Services;

namespace MusicTutor.Api.EFCore.Handlers.Pupils
{
    public record GetPupilLessonsHandler(IMusicTutorDbContext DbContext, IMapper Mapper) : IRequestHandler<GetPupilLessons, IEnumerable<LessonResponseDto>>
    {
        public async Task<IEnumerable<LessonResponseDto>> Handle(GetPupilLessons request, CancellationToken cancellationToken)
        {
            var pupil = await DbContext.GetPupilWithLessonsAsync(request.pupilId);

            if (pupil is null)
                return null;

            return Mapper.Map<IEnumerable<LessonResponseDto>>(pupil.Lessons);
        }
    }
}
