using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MapsterMapper;
using MediatR;
using MusicTutor.Api.Contracts.Instruments;
using MusicTutor.Api.Queries.Pupils;
using MusicTutor.Core.Services;
using MusicTutor.Api.Commands.Auth;

namespace MusicTutor.Api.EFCore.Handlers.Pupils
{
    public record GetPupilInstrumentsHandler(IMusicTutorDbContext DbContext, IMapper Mapper) : IRequestHandler<GetPupilInstruments, IEnumerable<InstrumentResponseDto>>
    {
        public async Task<IEnumerable<InstrumentResponseDto>> Handle(GetPupilInstruments request, CancellationToken cancellationToken)
        {
            var pupil = await DbContext.GetPupilWithInstrumentsAsync(request.pupilId);

            if (pupil is null)
                return null;

            return Mapper.Map<IEnumerable<InstrumentResponseDto>>(pupil.Instruments);
        }
    }
}
