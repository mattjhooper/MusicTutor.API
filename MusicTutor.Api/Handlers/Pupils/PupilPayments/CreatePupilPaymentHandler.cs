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
    public record CreatePupilPaymentHandler(IMusicTutorDbContext DbContext, IMapper Mapper) : IRequestHandler<CreatePupilPayment, PaymentResponseDto>
    {
        public async Task<PaymentResponseDto> Handle(CreatePupilPayment createPupilPayment, CancellationToken cancellationToken)
        {
            var pupil = await DbContext.Pupils.SingleOrDefaultAsync(p => p.Id == createPupilPayment.PupilId);
            if (pupil is null)
                return null;

            var payment = new Payment(createPupilPayment.PaymentDate, createPupilPayment.Amount, createPupilPayment.Type);
            pupil.AddPayment(payment);

            await DbContext.SaveChangesAsync(cancellationToken);

            return Mapper.Map<PaymentResponseDto>(payment);

            //var response = new PaymentResponseDto(Guid.NewGuid(), createPupilPayment.PaymentDate, createPupilPayment.Amount, createPupilPayment.Type);
            //return response;
        }
    }
}