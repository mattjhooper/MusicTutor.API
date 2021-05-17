using System.Collections.Generic;
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
    public record GetPupilPaymentsHandler(IMusicTutorDbContext DbContext, IMapper Mapper) : IRequestHandler<GetPupilPayments, IEnumerable<PaymentResponseDto>>
    {
        public async Task<IEnumerable<PaymentResponseDto>> Handle(GetPupilPayments request, CancellationToken cancellationToken)
        {
            var pupil = await DbContext.GetPupilWithPaymentsAsync(request.pupilId);

            if (pupil is null)
                return null;

            return Mapper.Map<IEnumerable<PaymentResponseDto>>(pupil.Payments);
        }
    }
}
