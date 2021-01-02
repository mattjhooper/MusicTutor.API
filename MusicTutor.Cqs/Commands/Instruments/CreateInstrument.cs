using MediatR;
using MusicTutor.Core.Contracts.Instruments;

namespace MusicTutor.Cqs.Commands.Instruments
{
    public record CreateInstrument(CreateInstrumentDto InstrumentToCreate) : IRequest<InstrumentResponseDto> {}     

}