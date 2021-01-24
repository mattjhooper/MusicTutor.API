using System;
using MediatR;
using MusicTutor.Api.Contracts.Pupils;

namespace MusicTutor.Api.Commands.Pupils
{
    public record DeletePupil(Guid Id) : IRequest<int> {}     

}