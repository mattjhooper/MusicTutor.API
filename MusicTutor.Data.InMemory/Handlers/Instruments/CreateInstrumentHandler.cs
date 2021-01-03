using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MusicTutor.Core.Contracts.Instruments;
using MusicTutor.Core.Models;
using MusicTutor.Core.Services;
using MusicTutor.Cqs.Commands.Instruments;
using MusicTutor.Data.InMemory.Services;

namespace MusicTutor.Data.InMemory.Handlers.Instruments
{
    public record CreateInstrumentHandler : BaseHandler, IRequestHandler<CreateInstrument, InstrumentResponseDto>
    {        
        public CreateInstrumentHandler(IDataService dataService) : base(dataService) {}
        
        public Task<InstrumentResponseDto> Handle(CreateInstrument request, CancellationToken cancellationToken)
        {
            var instrument = Instrument.CreateInstrument(request.InstrumentToCreate.Name);
            DataService.Instruments.Add(instrument);

            var dto = InstrumentResponseDto.MapFromInstrument(instrument);
            
            return Task.FromResult(dto);
        }
    }
}
    