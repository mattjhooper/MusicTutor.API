using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MusicTutor.Api.Commands.Pupils;
using MusicTutor.Api.Contracts.Payments;
using MusicTutor.Api.Queries.Pupils;
using MusicTutor.Core.Services;

namespace MusicTutor.Api.EFCore.Handlers.Pupils
{
    public record GetPupilPaymentByIdHandler(IMusicTutorDbContext DbContext, IMapper Mapper) : IRequestHandler<WithMusicTutorUserId<GetPupilPaymentById, PaymentResponseDto>, PaymentResponseDto>
    {
        public async Task<PaymentResponseDto> Handle(WithMusicTutorUserId<GetPupilPaymentById, PaymentResponseDto> request, CancellationToken cancellationToken)
        {
            var pupil = await DbContext.GetPupilWithPaymentsForUserAsync(request.Request.PupilId, request.MusicTutorUserId);

            if (pupil is null)
                return null;

            var payment = pupil.Payments.SingleOrDefault(l => l.Id == request.Request.PaymentId);

            if (payment is null)
                return null;

            return Mapper.Map<PaymentResponseDto>(payment);
        }
    }
}
