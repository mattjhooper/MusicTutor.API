using System;
using MediatR;
using MusicTutor.Api.Contracts.Instruments;

namespace MusicTutor.Api.Commands.Pupils
{
    public record CreatePupilInstrumentLink(Guid pupilId, Guid instrumentId) : IRequest<InstrumentResponseDto> {}     

}