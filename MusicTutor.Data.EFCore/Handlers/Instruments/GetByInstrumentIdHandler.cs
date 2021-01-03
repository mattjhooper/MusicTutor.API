using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MusicTutor.Core.Contracts.Instruments;
using MusicTutor.Cqs.Queries.Instruments;

namespace MusicTutor.Data.EFCore.Handlers.Instruments
{
    public record GetByInstrumentIdHandler(MusicTutorDbContext DbContext) : IRequestHandler<GetByInstrumentId, InstrumentResponseDto>
    {        
        public async Task<InstrumentResponseDto> Handle(GetByInstrumentId request, CancellationToken cancellationToken)
        {
            var instrument = await DbContext.Instruments.SingleOrDefaultAsync(i => i.Id == request.Id);

            InstrumentResponseDto response = instrument is null ? null : InstrumentResponseDto.MapFromInstrument(instrument);

            return response;
        }
    }
}
    