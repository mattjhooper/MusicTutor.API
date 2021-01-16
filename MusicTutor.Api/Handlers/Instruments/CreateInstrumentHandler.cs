using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MusicTutor.Api.Contracts.Instruments;
using MusicTutor.Core.Models;
using MusicTutor.Core.Services;
using MusicTutor.Api.Commands.Instruments;
using MapsterMapper;

namespace MusicTutor.Api.EFCore.Handlers.Instruments
{
    public record CreateInstrumentHandler(IMusicTutorDbContext DbContext, IMapper Mapper) : IRequestHandler<CreateInstrument, InstrumentResponseDto>
    {        
        public async Task<InstrumentResponseDto> Handle(CreateInstrument request, CancellationToken cancellationToken)
        {
            var instrument = new Instrument(request.Name);
            await DbContext.Instruments.AddAsync(instrument, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);

            return Mapper.Map<InstrumentResponseDto>(instrument);
        }
    }
}
    