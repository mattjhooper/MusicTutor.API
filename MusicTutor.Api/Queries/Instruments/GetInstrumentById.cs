using System;
using MediatR;
using MusicTutor.Api.Contracts.Instruments;

namespace MusicTutor.Api.Queries.Instruments
{
    public record GetInstrumentById(Guid Id) : IRequest<InstrumentResponseDto> {}     

}