using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MusicTutor.Core.Contracts.Instruments;
using MusicTutor.Cqs.Queries.Instruments;

namespace MusicTutor.Data.EFCore.Handlers.Instruments
{
    public record GetAllInstrumentsHandler(MusicTutorDbContext DbContext) : IRequestHandler<GetAllInstruments, IEnumerable<InstrumentResponseDto>>
    {        
        public Task<IEnumerable<InstrumentResponseDto>> Handle(GetAllInstruments request, CancellationToken cancellationToken)
        {
            IEnumerable<InstrumentResponseDto> response = DbContext.Instruments.AsQueryable().Select(i => InstrumentResponseDto.MapFromInstrument(i)).ToList();

            return Task.FromResult(response);
        }
    }
}
    