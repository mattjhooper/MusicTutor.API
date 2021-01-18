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


namespace MusicTutor.Api.UnitTests.Utils
{   
    public class MockDbContextBuilder
    {
        private IMusicTutorDbContext _dbContext = Substitute.For<IMusicTutorDbContext>();

        public static MockDbContextBuilder Init() => new MockDbContextBuilder();

        public IMusicTutorDbContext Build() => _dbContext;

        public MockDbContextBuilder WithInstruments()
        {
            var instruments = new List<Instrument>()
            {
                new Instrument("Piano"),
                new Instrument("Flute")
            };
            var mock = instruments.AsQueryable().BuildMockDbSet();
            _dbContext.Instruments.Returns(mock);

            return this;
        }
    }
}