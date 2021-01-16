using MediatR;
using MusicTutor.Api.Contracts.Instruments;
using System.Collections.Generic;

namespace MusicTutor.Api.Queries.Instruments
{
    public record GetAllInstruments : IRequest<IEnumerable<InstrumentResponseDto>> {}     

}