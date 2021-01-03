using System;
using MediatR;
using MusicTutor.Core.Contracts.Instruments;

namespace MusicTutor.Cqs.Queries.Instruments
{
    public record GetByInstrumentId(Guid Id) : IRequest<InstrumentResponseDto> {}     

}