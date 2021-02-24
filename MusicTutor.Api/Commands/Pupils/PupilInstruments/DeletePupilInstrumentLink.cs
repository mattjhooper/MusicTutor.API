using System;
using MediatR;
using MusicTutor.Api.Behaviors;

namespace MusicTutor.Api.Commands.Pupils
{
    public record DeletePupilInstrumentLink(Guid pupilId, Guid instrumentId) : IRequest<int>, IPipelineValidate {}     

}