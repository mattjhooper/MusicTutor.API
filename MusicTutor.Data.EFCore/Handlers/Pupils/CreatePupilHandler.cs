using System;
using System.Threading;
using System.Threading.Tasks;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MusicTutor.Core.Contracts.Pupils;
using MusicTutor.Core.Models;
using MusicTutor.Cqs.Commands.Pupils;

namespace MusicTutor.Data.EFCore.Handlers.Instruments
{
    public record CreatePupilHandler(MusicTutorDbContext DbContext, IMapper Mapper) : IRequestHandler<CreatePupil, PupilResponseDto>
    {        
        public async Task<PupilResponseDto> Handle(CreatePupil request, CancellationToken cancellationToken)
        {
            var p = request.PupilToCreate;
            var instrument = await DbContext.Instruments.SingleOrDefaultAsync(i => i.Id == p.DefaultInstrumentId);

            if (instrument is null)
                throw new InvalidOperationException("Instrument cannot be found");

            Pupil pupil = new Pupil(p.Name, p.LessonRate, p.StartDate, p.FrequencyInDays, new Instrument[] {instrument}, null);
            
            await DbContext.AddAsync<Pupil>(pupil);
            await DbContext.SaveChangesAsync();

            return Mapper.Map<PupilResponseDto>(pupil);
        }
    }
}
    