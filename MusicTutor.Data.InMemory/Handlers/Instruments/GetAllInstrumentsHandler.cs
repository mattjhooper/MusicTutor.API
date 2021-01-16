using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MusicTutor.Api.Contracts.Instruments;
using MusicTutor.Core.Services;
using MusicTutor.Api.Queries.Instruments;
using MusicTutor.Data.InMemory.Services;

namespace MusicTutor.Data.InMemory.Handlers.Instruments
{
    public record GetAllInstrumentsHandler : BaseHandler, IRequestHandler<GetAllInstruments, IEnumerable<InstrumentResponseDto>>
    {        
        public GetAllInstrumentsHandler(IDataService dataService) : base(dataService) {}        

        public Task<IEnumerable<InstrumentResponseDto>> Handle(GetAllInstruments request, CancellationToken cancellationToken)
        {
            var response = DataService.Instruments.Select(i => InstrumentResponseDto.MapFromInstrument(i));
            
            return Task.FromResult(response);
        }
    }
}
    