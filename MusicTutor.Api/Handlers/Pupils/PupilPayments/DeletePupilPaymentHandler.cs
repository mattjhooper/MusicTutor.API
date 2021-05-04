using System;
using System.Threading;
using System.Threading.Tasks;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MusicTutor.Api.Commands.Pupils;
using MusicTutor.Core.Services;
using MusicTutor.Api.Contracts.Payments;
using System.Linq;

namespace MusicTutor.Api.EFCore.Handlers.Pupils
{
    public record DeletePupilPaymentHandler(IMusicTutorDbContext DbContext) : IRequestHandler<WithMusicTutorUserId<DeletePupilPayment, int>, int>
    {
        public async Task<int> Handle(WithMusicTutorUserId<DeletePupilPayment, int> request, CancellationToken cancellationToken)
        {
            var pupil = await DbContext.GetPupilWithPaymentsForUserAsync(request.Request.PupilId, request.MusicTutorUserId);

            if (pupil is null)
                return -1;

            var payment = pupil.Payments.SingleOrDefault(l => l.Id == request.Request.PaymentId);

            if (payment is null)
                return 0;

            var isPaymentDeleted = pupil.RemovePayment(payment);

            if (isPaymentDeleted)
            {
                await DbContext.SaveChangesAsync(cancellationToken);
                return 1;
            }

            return 0;
        }
    }
}