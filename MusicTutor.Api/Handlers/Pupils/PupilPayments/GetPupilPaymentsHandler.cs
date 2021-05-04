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
    public record GetPupilPaymentsHandler(IMusicTutorDbContext DbContext, IMapper Mapper) : IRequestHandler<WithMusicTutorUserId<GetPupilPayments, IEnumerable<PaymentResponseDto>>, IEnumerable<PaymentResponseDto>>
    {
        public async Task<IEnumerable<PaymentResponseDto>> Handle(WithMusicTutorUserId<GetPupilPayments, IEnumerable<PaymentResponseDto>> request, CancellationToken cancellationToken)
        {
            var pupil = await DbContext.GetPupilWithPaymentsForUserAsync(request.Request.pupilId, request.MusicTutorUserId);

            if (pupil is null)
                return null;

            return Mapper.Map<IEnumerable<PaymentResponseDto>>(pupil.Payments);
        }
    }
}
