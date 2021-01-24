using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MusicTutor.Api.Contracts.Pupils;
using MusicTutor.Api.Queries.Pupils;
using MusicTutor.Core.Services;

namespace MusicTutor.Api.EFCore.Handlers.Pupils
{
    public record GetPupilByIdHandle(IMusicTutorDbContext DbContext, IMapper Mapper) : IRequestHandler<GetPupilById, PupilResponseDto>
    {        
        public async Task<PupilResponseDto> Handle(GetPupilById request, CancellationToken cancellationToken)
        {
            var pupil = await DbContext.Pupils.Where(p => p.Id == request.Id).ProjectToType<PupilResponseDto>().SingleOrDefaultAsync();

            return pupil;
        }
    }
}
     