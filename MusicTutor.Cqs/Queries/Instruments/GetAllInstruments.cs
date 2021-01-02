using MediatR;
using MusicTutor.Core.Contracts.Instruments;
using System.Collections.Generic;

namespace MusicTutor.Cqs.Queries.Instruments
{
    public record GetAllInstruments : IRequest<IEnumerable<InstrumentResponseDto>> {}     

}