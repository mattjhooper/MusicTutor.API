using FluentValidation;
using MusicTutor.Api.Commands.Instruments;
using MusicTutor.Core.Models;

namespace MusicTutor.Api.Validators.Instruments
{
    public class CreateInstrumentValidator : AbstractValidator<CreateInstrument>
    {
        public CreateInstrumentValidator()
        {
            RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(Instrument.NameLength);
        }
    }
}