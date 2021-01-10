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
            var pupil = await DbContext.Pupils.Where(p => p.Id == request.Id).ProjectToType<PupilResponseDto>().FirstOrDefaultAsync();

            return pupil;
        }
    }
}
    