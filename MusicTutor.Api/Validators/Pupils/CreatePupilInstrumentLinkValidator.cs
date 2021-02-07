using FluentValidation;
using MusicTutor.Api.Commands.Pupils;

namespace MusicTutor.Api.Validators.Pupils
{
    public class CreatePupilInstrumentLinkValidator : AbstractValidator<CreatePupilInstrumentLink>
    {
        private readonly IDbValidator check;
        public CreatePupilInstrumentLinkValidator(IDbValidator dbValidator)
        {
            check = dbValidator;

            RuleFor(x => x.pupilId)
             .NotNull()
             .NotEmpty();

            RuleFor(x => x.instrumentId)
             .NotNull()
             .NotEmpty()
             .MustAsync(check.InstrumentAlreadyExistsAsync).WithMessage("Instrument must exist. No matching instrument for supplied instrumentId.");
        }
    }
}