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
        private readonly IDbValidator check;
        public CreatePupilValidator(IDbValidator dbValidator)
        {
            check = dbValidator;

            RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(Pupil.NameLength);

            RuleFor(x => x.DefaultInstrumentId)
             .NotNull()
             .MustAsync(check.InstrumentAlreadyExistsAsync).WithMessage("Instrument must exist. No matching instrument for supplied DefaultInstrumentId.");
        }
    }
}