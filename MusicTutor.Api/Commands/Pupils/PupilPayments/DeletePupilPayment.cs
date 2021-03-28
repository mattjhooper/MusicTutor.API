using System;
using MediatR;
using MusicTutor.Api.Contracts.Payments;

namespace MusicTutor.Api.Commands.Pupils
{
    public record DeletePupilPayment(Guid PupilId, Guid PaymentId) : IRequest<int> { }

}