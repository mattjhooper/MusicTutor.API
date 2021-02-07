using System;
using FluentValidation.TestHelper;
using MusicTutor.Api.Commands.Pupils;
using MusicTutor.Api.Validators.Pupils;
using NSubstitute;
using Xunit;

namespace MusicTutor.Api.UnitTests.Validators.Pupils
{
    public class CreatePupilInstrumentLinkValidatorUnitTests
    {
        private readonly CreatePupilInstrumentLinkValidator validator;
        private readonly IDbValidator dbValidator;
        public CreatePupilInstrumentLinkValidatorUnitTests()
        {
            dbValidator = Substitute.For<IDbValidator>();
            dbValidator.InstrumentAlreadyExistsAsync(Arg.Any<Guid>(), default).Returns(true);
            
            validator = new CreatePupilInstrumentLinkValidator(dbValidator);

        }

        [Fact]
        public void EmptyPupilId_Invalid()
        {
            //Given
            var createPupilInstrumentLink = new CreatePupilInstrumentLink(Guid.Empty, Guid.NewGuid());
            
            //When
            var result = validator.TestValidate(createPupilInstrumentLink);
            
            //Then
            result.ShouldHaveValidationErrorFor(i => i.pupilId);
        }

        [Fact]
        public void EmptyInstrumentId_Invalid()
        {
            //Given
            var createPupilInstrumentLink = new CreatePupilInstrumentLink(Guid.NewGuid(), Guid.Empty);
            
            //When
            var result = validator.TestValidate(createPupilInstrumentLink);
            
            //Then
            result.ShouldHaveValidationErrorFor(i => i.instrumentId);
        }

        [Fact]
        public void UnknownInstrumentId_Invalid()
        {
            //Given
            dbValidator.InstrumentAlreadyExistsAsync(Arg.Any<Guid>(), default).Returns(false);
            var createPupilInstrumentLink = new CreatePupilInstrumentLink(Guid.NewGuid(), Guid.NewGuid());
            
            //When
            var result = validator.TestValidate(createPupilInstrumentLink);
            
            //Then
            result.ShouldHaveValidationErrorFor(i => i.instrumentId);
        }
    }
}