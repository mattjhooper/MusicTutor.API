using System;
using System.Collections.Generic;
using MediatR;
using MusicTutor.Api.Contracts.Payments;

namespace MusicTutor.Api.Queries.Pupils
{
    public record GetPupilPayments(Guid pupilId) : IRequest<IEnumerable<PaymentResponseDto>> { }

}