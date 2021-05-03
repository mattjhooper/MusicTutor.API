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
    public record MusicTutorUserCreatePupilHandler(IMusicTutorDbContext DbContext, IMapper Mapper) : IRequestHandler<MusicTutorUserCreatePupil, PupilResponseDto>
    {
        public async Task<PupilResponseDto> Handle(MusicTutorUserCreatePupil request, CancellationToken cancellationToken)
        {
            var instrument = await DbContext.Instruments.SingleOrDefaultAsync(i => i.Id == request.CreatePupil.DefaultInstrumentId);

            if (instrument is null)
                throw new InvalidOperationException("Instrument cannot be found");

            Pupil pupil = request.CreatePupil.MakePupil(instrument);
            pupil.AssignToMusicTutorUser(request.MusicTutorUserId);

            await DbContext.Pupils.AddAsync(pupil, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);

            return Mapper.Map<PupilResponseDto>(pupil);
        }
    }
}
