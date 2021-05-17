using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MusicTutor.Api.Commands.Pupils;
using MusicTutor.Core.Services;
using System.Linq;

namespace MusicTutor.Api.EFCore.Handlers.Pupils
{
    public record DeletePupilPaymentHandler(IMusicTutorDbContext DbContext) : IRequestHandler<DeletePupilPayment, int>
    {
        public async Task<int> Handle(DeletePupilPayment request, CancellationToken cancellationToken)
        {
            var pupil = await DbContext.GetPupilWithPaymentsAsync(request.PupilId);

            if (pupil is null)
                return -1;

            var payment = pupil.Payments.SingleOrDefault(l => l.Id == request.PaymentId);

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