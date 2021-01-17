using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MusicTutor.Api.Commands.Pupils;
using MusicTutor.Core.Models;
using MusicTutor.Core.Services;

namespace MusicTutor.Api.Validators.Pupils
{
    public class CreatePupilValidator : AbstractValidator<CreatePupil>
    {
        private readonly IMusicTutorDbContext _dbContext;
        public CreatePupilValidator(IMusicTutorDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(Pupil.NameLength);

            RuleFor(x => x.DefaultInstrumentId)
             .NotNull()
             .MustAsync(AlreadyExist).WithMessage("Instrument must exist. No matching instrument for supplied DefaultInstrumentId.");
        }

        public async Task<bool> AlreadyExist(Guid instrumentId, CancellationToken cancellationToken)
        {
            return await _dbContext.Instruments.AnyAsync(i => i.Id == instrumentId);
        }
    }
}