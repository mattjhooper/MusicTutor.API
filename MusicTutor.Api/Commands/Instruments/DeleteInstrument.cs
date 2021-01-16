using System;
using MediatR;
using MusicTutor.Api.Contracts.Instruments;

namespace MusicTutor.Api.Commands.Instruments
{
    public record DeleteInstrument(Guid Id) : IRequest<int> {}     

}