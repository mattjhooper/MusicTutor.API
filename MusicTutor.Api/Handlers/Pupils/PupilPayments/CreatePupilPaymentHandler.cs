using System.Threading;
using System.Threading.Tasks;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MusicTutor.Api.Commands.Pupils;
using MusicTutor.Core.Services;
using MusicTutor.Api.Contracts.Payments;
using MusicTutor.Core.Models;
using System;

namespace MusicTutor.Api.EFCore.Handlers.Pupils
{
    public record CreatePupilPaymentHandler(IMusicTutorDbContext DbContext, IMapper Mapper) : IRequestHandler<WithMusicTutorUserId<CreatePupilPayment, PaymentResponseDto>, PaymentResponseDto>
    {
        public async Task<PaymentResponseDto> Handle(WithMusicTutorUserId<CreatePupilPayment, PaymentResponseDto> request, CancellationToken cancellationToken)
        {
            var createPupilPayment = request.Request;
            var pupil = await DbContext.GetPupilWithPaymentsForUserAsync(createPupilPayment.PupilId, request.MusicTutorUserId);

            if (pupil is null)
                return null;

            var payment = new Payment(createPupilPayment.PaymentDate, createPupilPayment.Amount, createPupilPayment.Type);
            pupil.AddPayment(payment);

            await DbContext.SaveChangesAsync(cancellationToken);

            return Mapper.Map<PaymentResponseDto>(payment);
        }
    }
}