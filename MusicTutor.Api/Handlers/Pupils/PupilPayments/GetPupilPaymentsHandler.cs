using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MusicTutor.Api.Contracts.Payments;
using MusicTutor.Api.Queries.Pupils;
using MusicTutor.Core.Services;

namespace MusicTutor.Api.EFCore.Handlers.Pupils
{
    public record GetPupilPaymentsHandler(IMusicTutorDbContext DbContext, IMapper Mapper) : IRequestHandler<GetPupilPayments, IEnumerable<PaymentResponseDto>>
    {
        public async Task<IEnumerable<PaymentResponseDto>> Handle(GetPupilPayments request, CancellationToken cancellationToken)
        {
            var pupil = await DbContext.Pupils.Where(p => p.Id == request.pupilId).Include(p => p.Payments).SingleOrDefaultAsync();

            if (pupil is null)
                return null;

            return Mapper.Map<IEnumerable<PaymentResponseDto>>(pupil.Payments);
        }
    }
}
