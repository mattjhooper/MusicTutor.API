using FluentValidation;
using MusicTutor.Api.Commands.Pupils;

namespace MusicTutor.Api.Validators.Pupils
{
    public class DeletePupilInstrumentLinkValidator : AbstractValidator<DeletePupilInstrumentLink>
    {
        private readonly IDbValidator check;
        public DeletePupilInstrumentLinkValidator(IDbValidator dbValidator)
        {
            check = dbValidator;

            RuleFor(x => x.pupilId)
             .NotNull()
             .NotEmpty();

            RuleFor(x => x.instrumentId)
             .NotNull()
             .NotEmpty();

            RuleFor(x => x)
             .MustAsync(check.PupilInstrumentCanBeRemovedAsync).WithMessage("Instrument cannot be removed. Either the instrument is not assigned or is the last instrument.");
        }
    }
}