using FluentValidation;
using MusicTutor.Api.Commands.Instruments;
using MusicTutor.Core.Models;

namespace MusicTutor.Api.Validators.Instuments
{
    public class CreateInstrumentDtoValidator : AbstractValidator<CreateInstrument>
    {
        public CreateInstrumentDtoValidator()
        {
            RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(Instrument.NameLength);
        }
    }
}