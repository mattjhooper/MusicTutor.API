using FluentValidation;
using MusicTutor.Core.Contracts.Instruments;
using MusicTutor.Core.Models;

namespace MusicTutor.Api.Validators.Instuments
{
    public class CreateInstrumentDtoValidator : AbstractValidator<CreateInstrumentDto>
    {
        public CreateInstrumentDtoValidator()
        {
            RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(Instrument.NameLength);

            RuleFor(x => x.InstrumentType).IsEnumName(typeof(InstrumentType), caseSensitive: false);
        }
    }
}