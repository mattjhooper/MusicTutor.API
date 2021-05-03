using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MusicTutor.Api.Commands.Pupils;
using MusicTutor.Api.Contracts.Instruments;
using MusicTutor.Api.Contracts.Pupils;
using MusicTutor.Api.Queries.Pupils;
using MusicTutor.Core.Services;

namespace MusicTutor.Api.EFCore.Handlers.Pupils
{
    public record GetPupilInstrumentsHandler(IMusicTutorDbContext DbContext, IMapper Mapper) : IRequestHandler<WithMusicTutorUserId<GetPupilInstruments, IEnumerable<InstrumentResponseDto>>, IEnumerable<InstrumentResponseDto>>
    {
        public async Task<IEnumerable<InstrumentResponseDto>> Handle(WithMusicTutorUserId<GetPupilInstruments, IEnumerable<InstrumentResponseDto>> request, CancellationToken cancellationToken)
        {
            var pupil = await DbContext.GetPupilWithInstrumentsForUserAsync(request.Request.pupilId, request.MusicTutorUserId);

            if (pupil is null)
                return null;

            return Mapper.Map<IEnumerable<InstrumentResponseDto>>(pupil.Instruments);
        }
    }
}
