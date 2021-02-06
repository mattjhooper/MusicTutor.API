using System;
using System.Collections.Generic;
using MediatR;
using MusicTutor.Api.Contracts.Instruments;
using MusicTutor.Api.Contracts.Pupils;

namespace MusicTutor.Api.Queries.Pupils
{
    public record GetPupilInstruments(Guid pupilId) : IRequest<IEnumerable<InstrumentResponseDto>> {}     

}