using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MusicTutor.Api.Commands.Pupils;
using MusicTutor.Api.Contracts.Lessons;
using MusicTutor.Api.Queries.Pupils;
using MusicTutor.Core.Services;

namespace MusicTutor.Api.EFCore.Handlers.Pupils
{
    public record GetPupilLessonByIdHandler(IMusicTutorDbContext DbContext, IMapper Mapper) : IRequestHandler<WithMusicTutorUserId<GetPupilLessonById, LessonResponseDto>, LessonResponseDto>
    {
        public async Task<LessonResponseDto> Handle(WithMusicTutorUserId<GetPupilLessonById, LessonResponseDto> request, CancellationToken cancellationToken)
        {
            var getPupilLessonById = request.Request;
            var pupil = await DbContext.GetPupilWithLessonsForUserAsync(getPupilLessonById.PupilId, request.MusicTutorUserId);

            if (pupil is null)
                return null;

            var lesson = pupil.Lessons.SingleOrDefault(l => l.Id == getPupilLessonById.LessonId);

            if (lesson is null)
                return null;

            return Mapper.Map<LessonResponseDto>(lesson);
        }
    }
}
