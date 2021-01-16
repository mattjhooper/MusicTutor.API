using MediatR;
using MusicTutor.Api.Contracts.Instruments;

namespace MusicTutor.Api.Commands.Instruments
{
    public record CreateInstrument(CreateInstrumentDto InstrumentToCreate) : IRequest<InstrumentResponseDto> {}     

}