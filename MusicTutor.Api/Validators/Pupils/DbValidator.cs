using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicTutor.Api.Commands.Pupils;
using MusicTutor.Core.Services;

namespace MusicTutor.Api.Validators.Pupils
{
    public class DbValidator : IDbValidator
    {
        private readonly IMusicTutorDbContext _dbContext;
        public DbValidator(IMusicTutorDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<bool> InstrumentAlreadyExistsAsync(Guid instrumentId, CancellationToken cancellationToken)
        {
            return await _dbContext.Instruments.AnyAsync(i => i.Id == instrumentId);
        }

        public async Task<bool> PupilAlreadyExistsAsync(Guid pupilId, CancellationToken cancellationToken)
        {
            return await _dbContext.Pupils.AnyAsync(i => i.Id == pupilId);
        } 

        public async Task<bool> PupilInstrumentCanBeRemovedAsync(DeletePupilInstrumentLink deletePupilInstrumentLink, CancellationToken cancellationToken)
        {
            var pupil = await _dbContext.Pupils.Include(p => p.Instruments).SingleOrDefaultAsync(p => p.Id == deletePupilInstrumentLink.pupilId);
            if (pupil is null)                
                return false;

            return pupil.CanRemoveInstrument(deletePupilInstrumentLink.instrumentId);
        }
    }
}