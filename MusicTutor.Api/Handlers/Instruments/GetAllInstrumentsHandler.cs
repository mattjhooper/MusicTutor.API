using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MusicTutor.Api.Contracts.Instruments;
using MusicTutor.Core.Services;
using MusicTutor.Api.Queries.Instruments;

namespace MusicTutor.Api.EFCore.Handlers.Instruments
{
    public record GetAllInstrumentsHandler(IMusicTutorDbContext DbContext) : IRequestHandler<GetAllInstruments, IEnumerable<InstrumentResponseDto>>
    {        
        public async Task<IEnumerable<InstrumentResponseDto>> Handle(GetAllInstruments request, CancellationToken cancellationToken)
        {
            IEnumerable<InstrumentResponseDto> response = await DbContext.Instruments.Select(i => InstrumentResponseDto.MapFromInstrument(i)).ToListAsync();

            return response;
        }
    }
}
    