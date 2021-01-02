using MediatR;
using MusicTutor.Core.Contracts.Instruments;

namespace MusicTutor.Cqs.Queries.Instruments
{
    public record GetByInstrumentId(int Id) : IRequest<InstrumentResponseDto> {}     

}