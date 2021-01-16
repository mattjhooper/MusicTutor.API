using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MusicTutor.Api.Contracts.Instruments;
using MusicTutor.Core.Models;
using MusicTutor.Core.Services;
using MusicTutor.Api.Commands.Instruments;

namespace MusicTutor.Api.EFCore.Handlers.Instruments
{
    public record CreateInstrumentHandler(IMusicTutorDbContext DbContext) : IRequestHandler<CreateInstrument, InstrumentResponseDto>
    {        
        public async Task<InstrumentResponseDto> Handle(CreateInstrument request, CancellationToken cancellationToken)
        {
            var instrument = request.InstrumentToCreate.MapToInstrument();
            await DbContext.Instruments.AddAsync(instrument, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);

            var dto = InstrumentResponseDto.MapFromInstrument(instrument);
            
            return dto;
        }
    }
}
    