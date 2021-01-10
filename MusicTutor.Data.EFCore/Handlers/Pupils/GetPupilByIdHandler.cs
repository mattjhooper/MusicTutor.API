using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MusicTutor.Core.Contracts.Pupils;
using MusicTutor.Cqs.Queries.Pupils;

namespace MusicTutor.Data.EFCore.Handlers.Pupils
{
    public record GetPupilByIdHandle(MusicTutorDbContext DbContext, IMapper Mapper) : IRequestHandler<GetPupilById, PupilResponseDto>
    {        
        public async Task<PupilResponseDto> Handle(GetPupilById request, CancellationToken cancellationToken)
        {
            var pupil = await DbContext.Pupils.SingleOrDefaultAsync(i => i.Id == request.Id);

            PupilResponseDto response = pupil is null ? null : Mapper.Map<PupilResponseDto>(pupil);

            return response;
        }
    }
}
    