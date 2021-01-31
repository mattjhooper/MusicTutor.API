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
    public class UpdatePupilValidator : AbstractValidator<UpdatePupil>
    {
        private readonly IDbValidator check;
        public UpdatePupilValidator(IDbValidator dbValidator)
        {
            check = dbValidator;

            RuleFor(x => x.Id)
             .NotNull()
             .MustAsync(check.PupilAlreadyExistsAsync).WithMessage("Pupil must exist. No matching Pupil for supplied Id.");

            RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(Pupil.NameLength);

            
        }
    }
}