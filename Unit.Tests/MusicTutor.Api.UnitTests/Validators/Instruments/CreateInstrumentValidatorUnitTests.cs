using FluentValidation;
using FluentValidation.TestHelper;
using MusicTutor.Api.Commands.Instruments;
using MusicTutor.Core.Models;
using MusicTutor.Api.Validators.Instruments;
using FluentAssertions;
using Xunit;

namespace MusicTutor.Api.UnitTests.Validators.Instruments
{
    public class CreateInstrumentValidatorUnitTests
    {
        private readonly CreateInstrumentValidator validator;
        public CreateInstrumentValidatorUnitTests()
        {
            validator = new CreateInstrumentValidator();            
        }

        [Fact]
        public void EmptyName_Invalid()
        {
            //Given
            var createInstrument = new CreateInstrument(null);
            
            //When
            var result = validator.TestValidate(createInstrument);
            
            //Then
            result.ShouldHaveValidationErrorFor(i => i.Name);
        }

        [Fact]
        public void Name_Valid()
        {
            //Given
            var createInstrument = new CreateInstrument("Piano");
            
            //When
            var result = validator.TestValidate(createInstrument);
            
            //Then
            result.ShouldNotHaveValidationErrorFor(i => i.Name);
        }
    }
}