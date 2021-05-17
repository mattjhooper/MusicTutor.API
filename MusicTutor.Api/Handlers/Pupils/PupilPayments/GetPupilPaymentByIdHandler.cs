using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MapsterMapper;
using MediatR;
using MusicTutor.Api.Commands.Auth;
using MusicTutor.Api.Contracts.Payments;
using MusicTutor.Api.Queries.Pupils;
using MusicTutor.Core.Services;

namespace MusicTutor.Api.EFCore.Handlers.Pupils
{
    public record GetPupilPaymentByIdHandler(IMusicTutorDbContext DbContext, IMapper Mapper) : IRequestHandler<GetPupilPaymentById, PaymentResponseDto>
    {
        public async Task<PaymentResponseDto> Handle(GetPupilPaymentById request, CancellationToken cancellationToken)
        {
            var pupil = await DbContext.GetPupilWithPaymentsAsync(request.PupilId);

            if (pupil is null)
                return null;

            var payment = pupil.Payments.SingleOrDefault(l => l.Id == request.PaymentId);

            if (payment is null)
                return null;

            return Mapper.Map<PaymentResponseDto>(payment);
        }
    }
}
