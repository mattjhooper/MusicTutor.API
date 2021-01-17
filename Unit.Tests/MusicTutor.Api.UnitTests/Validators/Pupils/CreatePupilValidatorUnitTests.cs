using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.TestHelper;
using MockQueryable.NSubstitute;
using MusicTutor.Api.Commands.Pupils;
using MusicTutor.Api.Validators.Pupils;
using MusicTutor.Core.Models;
using MusicTutor.Core.Services;
using NSubstitute;
using Xunit;

namespace MusicTutor.Api.UnitTests.Validators.Pupils
{
    public class CreatePupilValidatorUnitTests
    {
        private readonly CreatePupilValidator validator;
        private readonly IMusicTutorDbContext dbContext;
        public CreatePupilValidatorUnitTests()
        {
            var instruments = new List<Instrument>()
            {
                new Instrument("Piano"),
                new Instrument("Flute")
            };
            
            var mock = instruments.AsQueryable().BuildMockDbSet();   

            dbContext = Substitute.For<IMusicTutorDbContext>();
            dbContext.Instruments.Returns(mock);
            
            validator = new CreatePupilValidator(dbContext);

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