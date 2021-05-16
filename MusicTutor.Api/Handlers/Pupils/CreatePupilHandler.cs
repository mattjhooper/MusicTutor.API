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
using MusicTutor.Api.Commands.Auth;

namespace MusicTutor.Api.EFCore.Handlers.Pupils
{
    public record CreatePupilHandler(IMusicTutorDbContext DbContext, IMapper Mapper) : IRequestHandler<CreatePupil, PupilResponseDto>
    {
        public async Task<PupilResponseDto> Handle(CreatePupil request, CancellationToken cancellationToken)
        {
            var instrument = await DbContext.Instruments.SingleOrDefaultAsync(i => i.Id == request.DefaultInstrumentId);

            if (instrument is null)
                throw new InvalidOperationException("Instrument cannot be found");

            Pupil pupil = request.MakePupil(instrument);
            pupil.AssignToMusicTutorUser(DbContext.CurrentUserId);

            await DbContext.Pupils.AddAsync(pupil, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);

            return Mapper.Map<PupilResponseDto>(pupil);
        }
    }
}
