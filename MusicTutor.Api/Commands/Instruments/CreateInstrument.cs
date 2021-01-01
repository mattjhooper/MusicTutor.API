using MediatR;
using MusicTutor.Api.Controllers.Instruments.Dtos;

namespace MusicTutor.Api.Commands.Instruments
{
    public record CreateInstrument(CreateInstrumentDto InstrumentToCreate) : IRequest<InstrumentResponseDto> {}     

}