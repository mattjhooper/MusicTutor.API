using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MusicTutor.Api.Contracts.Instruments;
using MusicTutor.Core.Services;
using MusicTutor.Api.Queries.Instruments;

namespace MusicTutor.Api.EFCore.Handlers.Instruments
{
    public record GetInstrumentByIdHandler(IMusicTutorDbContext DbContext) : IRequestHandler<GetInstrumentById, InstrumentResponseDto>
    {        
        public async Task<InstrumentResponseDto> Handle(GetInstrumentById request, CancellationToken cancellationToken)
        {
            var instrument = await DbContext.Instruments.SingleOrDefaultAsync(i => i.Id == request.Id);

            InstrumentResponseDto response = instrument is null ? null : InstrumentResponseDto.MapFromInstrument(instrument);

            return response;
        }
    }
}
    