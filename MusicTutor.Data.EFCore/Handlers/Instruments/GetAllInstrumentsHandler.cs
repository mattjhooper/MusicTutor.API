using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MusicTutor.Core.Contracts.Instruments;
using MusicTutor.Cqs.Queries.Instruments;

namespace MusicTutor.Data.EFCore.Handlers.Instruments
{
    public record GetAllInstrumentsHandler(MusicTutorDbContext DbContext) : IRequestHandler<GetAllInstruments, IEnumerable<InstrumentResponseDto>>
    {        
        public async Task<IEnumerable<InstrumentResponseDto>> Handle(GetAllInstruments request, CancellationToken cancellationToken)
        {
            IEnumerable<InstrumentResponseDto> response = await DbContext.Instruments.Select(i => InstrumentResponseDto.MapFromInstrument(i)).ToListAsync();

            return response;
        }
    }
}
    