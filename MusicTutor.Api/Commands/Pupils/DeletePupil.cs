using System;
using MediatR;

namespace MusicTutor.Api.Commands.Pupils
{
    public record DeletePupil(Guid Id) : IRequest<int> {}     

}