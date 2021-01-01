using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MusicTutor.Api.Controllers.Instruments.Dtos;
using MusicTutor.Api.Queries.Instruments;
using MusicTutor.Data;

namespace MusicTutor.Api.Handlers.Instruments
{
    public record GetByInstrumentIdHandler(MusicTutorDbContext DbContext) : IRequestHandler<GetByInstrumentId, InstrumentResponseDto>
    {        
        public Task<InstrumentResponseDto> Handle(GetByInstrumentId request, CancellationToken cancellationToken)
        {
            var instrument = DbContext.Instruments.SingleOrDefault(i => i.Id == request.Id);

            InstrumentResponseDto response = instrument is null ? null : InstrumentResponseDto.MapFromInstrument(instrument);

            return Task.FromResult(response);
        }
    }
}
    