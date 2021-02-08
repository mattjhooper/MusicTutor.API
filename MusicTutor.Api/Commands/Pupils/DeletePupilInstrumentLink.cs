using System;
using MediatR;

namespace MusicTutor.Api.Commands.Pupils
{
    public record DeletePupilInstrumentLink(Guid pupilId, Guid instrumentId) : IRequest<int> {}     

}