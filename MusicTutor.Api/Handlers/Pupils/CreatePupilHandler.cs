using System;
using System.Threading;
using System.Threading.Tasks;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MusicTutor.Api.Contracts.Pupils;
using MusicTutor.Core.Models;
using MusicTutor.Api.Commands.Pupils;
using MusicTutor.Core.Services;

namespace MusicTutor.Api.EFCore.Handlers.Pupils
{
    public record CreatePupilHandler(IMusicTutorDbContext DbContext, IMapper Mapper) : IRequestHandler<WithMusicTutorUserId<CreatePupil, PupilResponseDto>, PupilResponseDto>
    {
        public async Task<PupilResponseDto> Handle(WithMusicTutorUserId<CreatePupil, PupilResponseDto> requestWithUserId, CancellationToken cancellationToken)
        {
            var instrument = await DbContext.Instruments.SingleOrDefaultAsync(i => i.Id == requestWithUserId.Request.DefaultInstrumentId);

            if (instrument is null)
                throw new InvalidOperationException("Instrument cannot be found");

            Pupil pupil = requestWithUserId.Request.MakePupil(instrument);
            pupil.AssignToMusicTutorUser(requestWithUserId.MusicTutorUserId);

            await DbContext.Pupils.AddAsync(pupil, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);

            return Mapper.Map<PupilResponseDto>(pupil);
        }
    }
}
