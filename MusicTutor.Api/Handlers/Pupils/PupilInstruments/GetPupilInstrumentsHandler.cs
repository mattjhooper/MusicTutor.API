using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MusicTutor.Api.Contracts.Instruments;
using MusicTutor.Api.Contracts.Pupils;
using MusicTutor.Api.Queries.Pupils;
using MusicTutor.Core.Services;

namespace MusicTutor.Api.EFCore.Handlers.Pupils
{
    public record GetPupilInstrumentsHandler(IMusicTutorDbContext DbContext, IMapper Mapper) : IRequestHandler<GetPupilInstruments, IEnumerable<InstrumentResponseDto>>
    {
        public async Task<IEnumerable<InstrumentResponseDto>> Handle(GetPupilInstruments request, CancellationToken cancellationToken)
        {
            // var pupilInstruments = await DbContext.Pupils
            //     .Where(p => p.Id == request.pupilId)
            //     .Include(p => p.Instruments)
            //     .SelectMany(p => p.Instruments)
            //     .ProjectToType<InstrumentResponseDto>()
            //     .ToListAsync();

            // return pupilInstruments;

            var pupil = await DbContext.Pupils.Where(p => p.Id == request.pupilId).Include(p => p.Instruments).SingleOrDefaultAsync();

            if (pupil is null)
                return null;

            return Mapper.Map<IEnumerable<InstrumentResponseDto>>(pupil.Instruments);
        }
    }
}
