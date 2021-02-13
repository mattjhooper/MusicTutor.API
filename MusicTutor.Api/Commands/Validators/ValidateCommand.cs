using System;
using MediatR;
using MusicTutor.Api.Contracts.Errors;
using MusicTutor.Api.Commands.Pupils;

namespace MusicTutor.Api.Commands.Validators
{
    public record ValidateCommand(DeletePupilInstrumentLink Command) : IRequest<ErrorResponse> {}     

}