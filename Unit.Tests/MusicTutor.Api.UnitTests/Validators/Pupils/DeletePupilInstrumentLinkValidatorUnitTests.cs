using System;
using FluentValidation.TestHelper;
using MusicTutor.Api.Commands.Pupils;
using MusicTutor.Api.Validators.Pupils;
using NSubstitute;
using Xunit;

namespace MusicTutor.Api.UnitTests.Validators.Pupils
{
    public class DeletePupilInstrumentLinkValidatorUnitTests
    {
        private readonly DeletePupilInstrumentLinkValidator validator;
        private readonly IDbValidator dbValidator;
        public DeletePupilInstrumentLinkValidatorUnitTests()
        {
            dbValidator = Substitute.For<IDbValidator>();
            dbValidator.PupilInstrumentCanBeRemovedAsync(Arg.Any<DeletePupilInstrumentLink>(), default).Returns(true);
            
            validator = new DeletePupilInstrumentLinkValidator(dbValidator);

        }

        [Fact]
        public void EmptyPupilId_Invalid()
        {
            //Given
            var deletePupilInstrumentLink = new DeletePupilInstrumentLink(Guid.Empty, Guid.NewGuid());
            
            //When
            var result = validator.TestValidate(deletePupilInstrumentLink);
            
            //Then
            result.ShouldHaveValidationErrorFor(i => i.pupilId);
        }

        [Fact]
        public void EmptyInstrumentId_Invalid()
        {
            //Given
            var deletePupilInstrumentLink = new DeletePupilInstrumentLink(Guid.NewGuid(), Guid.Empty);
            
            //When
            var result = validator.TestValidate(deletePupilInstrumentLink);
            
            //Then
            result.ShouldHaveValidationErrorFor(i => i.instrumentId);
        }

        [Fact]
        public void PupilInstrumentCanBeRemoved_False_Invalid()
        {
            //Given
            dbValidator.PupilInstrumentCanBeRemovedAsync(Arg.Any<DeletePupilInstrumentLink>(), default).Returns(false);
            var deletePupilInstrumentLink = new DeletePupilInstrumentLink(Guid.NewGuid(), Guid.NewGuid());
            
            //When
            var result = validator.TestValidate(deletePupilInstrumentLink);
            
            //Then
            result.ShouldHaveValidationErrorFor(i => i);
        }
    }
}