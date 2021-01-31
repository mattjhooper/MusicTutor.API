using System;
using FluentValidation.TestHelper;
using MusicTutor.Api.Commands.Pupils;
using MusicTutor.Api.Validators.Pupils;
using NSubstitute;
using Xunit;

namespace MusicTutor.Api.UnitTests.Validators.Pupils
{
    public class UpdatePupilValidatorUnitTests
    {
        private readonly UpdatePupilValidator validator;
        private readonly IDbValidator dbValidator;
        public UpdatePupilValidatorUnitTests()
        {
            dbValidator = Substitute.For<IDbValidator>();
            dbValidator.PupilAlreadyExistsAsync(Arg.Any<Guid>(), default).Returns(true);
            
            validator = new UpdatePupilValidator(dbValidator);

        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void EmptyName_Invalid(string name)
        {
            //Given
            var updatePupil = new UpdatePupil(Guid.NewGuid(), name, 0M, DateTime.Today, 0, null, null, null);
            
            //When
            var result = validator.TestValidate(updatePupil);
            
            //Then
            result.ShouldHaveValidationErrorFor(i => i.Name);
        }

        [Fact]
        public void Name_Valid()
        {
            //Given
            var updatePupil = new UpdatePupil(Guid.NewGuid(), "John", 0M, DateTime.Today, 0, null, null, null);
            
            //When
            var result = validator.TestValidate(updatePupil);
            
            //Then
            result.ShouldNotHaveValidationErrorFor(i => i.Name);
        }
    }
}