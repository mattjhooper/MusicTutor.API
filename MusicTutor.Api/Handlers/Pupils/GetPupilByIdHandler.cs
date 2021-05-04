using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MusicTutor.Api.Commands.Auth;
using MusicTutor.Api.Contracts.Pupils;
using MusicTutor.Api.Queries.Pupils;
using MusicTutor.Core.Services;

namespace MusicTutor.Api.EFCore.Handlers.Pupils
{
    public record GetPupilByIdHandler(IMusicTutorDbContext DbContext, IMapper Mapper) : IRequestHandler<WithMusicTutorUserId<GetPupilById, PupilResponseDto>, PupilResponseDto>
    {
        public async Task<PupilResponseDto> Handle(WithMusicTutorUserId<GetPupilById, PupilResponseDto> requestWithUserId, CancellationToken cancellationToken)
        {
            var pupil = await DbContext.Pupils.Where(p => p.Id == requestWithUserId.Request.Id && p.MusicTutorUserId == requestWithUserId.MusicTutorUserId).ProjectToType<PupilResponseDto>().SingleOrDefaultAsync();

            return pupil;
        }
    }
}
