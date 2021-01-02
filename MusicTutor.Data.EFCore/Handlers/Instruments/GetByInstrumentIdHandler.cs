using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MusicTutor.Core.Contracts.Instruments;
using MusicTutor.Cqs.Queries.Instruments;

namespace MusicTutor.Data.EFCore.Handlers.Instruments
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
    