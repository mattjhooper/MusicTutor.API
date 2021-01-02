using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MusicTutor.Core.Contracts.Instruments;
using MusicTutor.Core.Models;
using MusicTutor.Cqs.Commands.Instruments;
using MusicTutor.Data.InMemory.Services;

namespace MusicTutor.Data.InMemory.Handlers.Instruments
{
    public record CreateInstrumentHandler(DataServiceInMemory dataService) : IRequestHandler<CreateInstrument, InstrumentResponseDto>
    {        
        public Task<InstrumentResponseDto> Handle(CreateInstrument request, CancellationToken cancellationToken)
        {
            var instrument = request.InstrumentToCreate.MapToInstrument();
            dataService.Instruments.Add(instrument);

            var dto = InstrumentResponseDto.MapFromInstrument(instrument);
            
            return Task.FromResult(dto);
        }
    }
}
    