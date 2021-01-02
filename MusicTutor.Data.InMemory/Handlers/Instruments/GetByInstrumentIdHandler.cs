using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MusicTutor.Core.Contracts.Instruments;
using MusicTutor.Cqs.Queries.Instruments;
using MusicTutor.Data.InMemory.Services;

namespace MusicTutor.Data.InMemory.Handlers.Instruments
{
    public record GetByInstrumentIdHandler(DataServiceInMemory dataService) : IRequestHandler<GetByInstrumentId, InstrumentResponseDto>
    {        
        public Task<InstrumentResponseDto> Handle(GetByInstrumentId request, CancellationToken cancellationToken)
        {
            var instrument = dataService.Instruments.AsQueryable().FirstOrDefault(i => i.Id == request.Id);

            InstrumentResponseDto response = instrument is null ? null : InstrumentResponseDto.MapFromInstrument(instrument);

            return Task.FromResult(response);
        }
    }
}
    