using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MusicTutor.Api.Commands.Instruments;
using MusicTutor.Api.Controllers.Instruments.Dtos;
using MusicTutor.Core.Models;
using MusicTutor.Data;

namespace MusicTutor.Api.Handlers.Instruments
{
    public record CreateInstrumentHandler(MusicTutorDbContext DbContext) : IRequestHandler<CreateInstrument, InstrumentResponseDto>
    {        
        public Task<InstrumentResponseDto> Handle(CreateInstrument request, CancellationToken cancellationToken)
        {
            var instrument = request.InstrumentToCreate.MapToInstrument();
            DbContext.Add<Instrument>(instrument);
            DbContext.SaveChanges();

            var dto = InstrumentResponseDto.MapFromInstrument(instrument);
            
            return Task.FromResult(dto);
        }
    }
}
    