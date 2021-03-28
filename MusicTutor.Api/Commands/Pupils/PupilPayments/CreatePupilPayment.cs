using System;
using MediatR;
using MusicTutor.Api.Contracts.Payments;
using MusicTutor.Core.Models.Enums;

namespace MusicTutor.Api.Commands.Pupils
{
    public record CreatePupilPayment(Guid PupilId, DateTime PaymentDate, decimal Amount, PaymentType Type) : IRequest<PaymentResponseDto> { }

}