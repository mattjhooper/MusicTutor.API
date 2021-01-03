using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MusicTutor.Core.Contracts.Instruments;
using MusicTutor.Core.Services;
using MusicTutor.Cqs.Queries.Instruments;
using MusicTutor.Data.InMemory.Services;

namespace MusicTutor.Data.InMemory.Handlers.Instruments
{
    public record GetAllInstrumentsHandler : BaseHandler, IRequestHandler<GetAllInstruments, IEnumerable<InstrumentResponseDto>>
    {        
        public GetAllInstrumentsHandler(IDataService dataService) : base(dataService) {}        

        public Task<IEnumerable<InstrumentResponseDto>> Handle(GetAllInstruments request, CancellationToken cancellationToken)
        {
            //IEnumerable<InstrumentResponseDto> response = dataService.Instruments.AsQueryable().Select<InstrumentResponseDto>(i => InstrumentResponseDto.MapFromInstrument(i)).ToList();            
            
            var instruments = new List<InstrumentResponseDto>();
            foreach(var i in DataService.Instruments)
            {
                instruments.Add(InstrumentResponseDto.MapFromInstrument(i));
            }

            IEnumerable<InstrumentResponseDto> response = instruments;

            return Task.FromResult(response);
        }
    }
}
    