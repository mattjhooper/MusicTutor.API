using System;
using System.Threading;
using System.Threading.Tasks;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MusicTutor.Api.Commands.Pupils;
using MusicTutor.Core.Services;
using MusicTutor.Api.Contracts.Instruments;

namespace MusicTutor.Api.EFCore.Handlers.Pupils
{
    public record CreatePupilInstrumentLinkHandler(IMusicTutorDbContext DbContext, IMapper Mapper) : IRequestHandler<WithMusicTutorUserId<CreatePupilInstrumentLink, InstrumentResponseDto>, InstrumentResponseDto>
    {
        public async Task<InstrumentResponseDto> Handle(WithMusicTutorUserId<CreatePupilInstrumentLink, InstrumentResponseDto> request, CancellationToken cancellationToken)
        {
            var createPupilInstrumentLink = request.Request;
            var pupil = await DbContext.GetPupilWithInstrumentsForUserAsync(createPupilInstrumentLink.pupilId, request.MusicTutorUserId);
            if (pupil is null)
                return null;

            var instrument = await DbContext.Instruments.SingleOrDefaultAsync(i => i.Id == createPupilInstrumentLink.instrumentId);

            if (instrument is null)
                throw new InvalidOperationException("Instrument cannot be found");

            pupil.AddInstrument(instrument);

            await DbContext.SaveChangesAsync(cancellationToken);

            return Mapper.Map<InstrumentResponseDto>(instrument);
        }
    }
}