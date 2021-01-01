using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MusicTutor.Api.Controllers.Instruments.Dtos;
using MusicTutor.Api.Queries.Instruments;
using MusicTutor.Data;

namespace MusicTutor.Api.Handlers.Instruments
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
    