using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MusicTutor.Core.Models;
using MusicTutor.Data;
using MusicTutor.Api.Controllers.Instruments.Dtos;

namespace MusicTutor.Api.Queries.Instruments
{
    public record GetSingleInstrumentRequest(int Id) : IRequest<InstrumentResponseDto> {}

    public record GetSingleInstrumentHandler(MusicTutorDbContext DbContext) : IRequestHandler<GetSingleInstrumentRequest, InstrumentResponseDto>
    {        
        public Task<InstrumentResponseDto> Handle(GetSingleInstrumentRequest request, CancellationToken cancellationToken)
        {
            var instrument = DbContext.Instruments.SingleOrDefault(i => i.Id == request.Id);

            return Task.FromResult(InstrumentResponseDto.MapFromInstrument(instrument));
        }
    } 
}