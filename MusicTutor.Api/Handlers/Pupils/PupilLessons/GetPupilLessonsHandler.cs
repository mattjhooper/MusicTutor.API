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
    public record GetPupilLessonsHandler(IMusicTutorDbContext DbContext, IMapper Mapper) : IRequestHandler<WithMusicTutorUserId<GetPupilLessons, IEnumerable<LessonResponseDto>>, IEnumerable<LessonResponseDto>>
    {
        public async Task<IEnumerable<LessonResponseDto>> Handle(WithMusicTutorUserId<GetPupilLessons, IEnumerable<LessonResponseDto>> request, CancellationToken cancellationToken)
        {
            var pupil = await DbContext.GetPupilWithLessonsForUserAsync(request.Request.pupilId, request.MusicTutorUserId);

            if (pupil is null)
                return null;

            return Mapper.Map<IEnumerable<LessonResponseDto>>(pupil.Lessons);
        }
    }
}
