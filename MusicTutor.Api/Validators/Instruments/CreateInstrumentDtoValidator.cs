using FluentValidation;
using MusicTutor.Api.Contracts.Instruments;
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
        }
    }
}