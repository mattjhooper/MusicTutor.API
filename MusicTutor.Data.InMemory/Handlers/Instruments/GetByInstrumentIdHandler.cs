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
    public record GetInstrumentByIdHandler: BaseHandler, IRequestHandler<GetInstrumentById, InstrumentResponseDto>
    {        
        public GetInstrumentByIdHandler(IDataService dataService) : base(dataService) {}

        public Task<InstrumentResponseDto> Handle(GetInstrumentById request, CancellationToken cancellationToken)
        {
            var instrument = DataService.Instruments.AsQueryable().FirstOrDefault(i => i.Id == request.Id);

            InstrumentResponseDto response = instrument is null ? null : InstrumentResponseDto.MapFromInstrument(instrument);

            return Task.FromResult(response);
        }
    }
}
    