using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MusicTutor.Core.Contracts.Instruments;
using MusicTutor.Core.Models;
using MusicTutor.Cqs.Commands.Instruments;

namespace MusicTutor.Data.EFCore.Handlers.Instruments
{
    public record CreateInstrumentHandler(MusicTutorDbContext DbContext) : IRequestHandler<CreateInstrument, InstrumentResponseDto>
    {        
        public async Task<InstrumentResponseDto> Handle(CreateInstrument request, CancellationToken cancellationToken)
        {
            var instrument = request.InstrumentToCreate.MapToInstrument();
            await DbContext.AddAsync<Instrument>(instrument);
            await DbContext.SaveChangesAsync();

            var dto = InstrumentResponseDto.MapFromInstrument(instrument);
            
            return dto;
        }
    }
}
    