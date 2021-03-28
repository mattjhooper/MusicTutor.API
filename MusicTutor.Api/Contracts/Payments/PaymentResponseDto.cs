using System;
using MusicTutor.Core.Models.Enums;

namespace MusicTutor.Api.Contracts.Payments
{
    public record PaymentResponseDto(Guid Id, DateTime PaymentDate, decimal Amount, PaymentType Type);
}
