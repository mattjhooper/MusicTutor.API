using System;
using System.Collections.Generic;
using MediatR;
using MusicTutor.Api.Contracts.Payments;

namespace MusicTutor.Api.Queries.Pupils
{
    public record GetPupilPaymentById(Guid PupilId, Guid PaymentId) : IRequest<PaymentResponseDto> { }

}