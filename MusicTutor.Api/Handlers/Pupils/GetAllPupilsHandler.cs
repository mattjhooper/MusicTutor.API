using System.Collections.Generic;
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
    public record GetAllPupilsHandler(IMusicTutorDbContext DbContext, IMapper Mapper) : IRequestHandler<WithMusicTutorUserId<GetAllPupils, IEnumerable<PupilResponseDto>>, IEnumerable<PupilResponseDto>>
    {
        public async Task<IEnumerable<PupilResponseDto>> Handle(WithMusicTutorUserId<GetAllPupils, IEnumerable<PupilResponseDto>> request, CancellationToken cancellationToken)
        {
            var pupils = await DbContext.Pupils.Where(p => p.MusicTutorUserId == request.MusicTutorUserId).AsNoTracking().ProjectToType<PupilResponseDto>().ToListAsync();

            return pupils;
        }
    }
}
