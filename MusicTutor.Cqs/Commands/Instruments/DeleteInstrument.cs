using System;
using MediatR;
using MusicTutor.Core.Contracts.Instruments;

namespace MusicTutor.Cqs.Commands.Instruments
{
    public record DeleteInstrument(Guid Id) : IRequest<int> {}     

}