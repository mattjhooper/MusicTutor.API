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
    public record CreatePupilHandler(IMusicTutorDbContext DbContext, IMapper Mapper) : IRequestHandler<CreatePupil, PupilResponseDto>
    {
        public async Task<PupilResponseDto> Handle(CreatePupil request, CancellationToken cancellationToken)
        {
            Instrument instrument = await GetInstrument(request);
            Pupil pupil = MakePupil(request, instrument);
            await SavePupil(pupil, cancellationToken);

            return Mapper.Map<PupilResponseDto>(pupil);
        }

        private Pupil MakePupil(CreatePupil request, Instrument instrument)
        {
            Pupil pupil = request.MakePupil(instrument);
            pupil.AssignToMusicTutorUser(DbContext.CurrentUserId);
            return pupil;
        }

        private async Task SavePupil(Pupil pupil, CancellationToken cancellationToken)
        {
            await DbContext.Pupils.AddAsync(pupil, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task<Instrument> GetInstrument(CreatePupil request)
        {
            var instrument = await DbContext.Instruments.SingleOrDefaultAsync(i => i.Id == request.DefaultInstrumentId);

            if (instrument is null)
                throw new InvalidOperationException("Instrument cannot be found");
            return instrument;
        }
    }
}
