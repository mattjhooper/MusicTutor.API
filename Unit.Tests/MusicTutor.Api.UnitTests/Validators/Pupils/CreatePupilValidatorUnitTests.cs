using System;
using FluentValidation.TestHelper;
using MusicTutor.Api.Commands.Pupils;
using MusicTutor.Api.Validators.Pupils;
using NSubstitute;
using Xunit;

namespace MusicTutor.Api.UnitTests.Validators.Pupils
{
    public class CreatePupilValidatorUnitTests
    {
        private readonly CreatePupilValidator validator;
        private readonly IDbValidator dbValidator;
        public CreatePupilValidatorUnitTests()
        {
            dbValidator = Substitute.For<IDbValidator>();
            dbValidator.InstrumentAlreadyExistsAsync(Arg.Any<Guid>(), default).Returns(true);
            
            validator = new CreatePupilValidator(dbValidator);

        }

        [Fact]
        public void EmptyName_Invalid()
        {
            //Given
            var createPupil = new CreatePupil(null, 0M, DateTime.Today, 0, Guid.NewGuid(), null, null, null);
            
            //When
            var result = validator.TestValidate(createPupil);
            
            //Then
            result.ShouldHaveValidationErrorFor(i => i.Name);
        }

        [Fact]
        public void Name_Valid()
        {
            //Given
            var createPupil = new CreatePupil("John", 0M, DateTime.Today, 0, Guid.NewGuid(), null, null, null);
            
            //When
            var result = validator.TestValidate(createPupil);
            
            //Then
            result.ShouldNotHaveValidationErrorFor(i => i.Name);
        }
    }
}