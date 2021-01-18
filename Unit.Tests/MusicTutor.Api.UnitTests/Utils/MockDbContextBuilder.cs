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

        private List<Instrument> _instruments;

        public MockDbContextBuilder()
        {
            _instruments = new List<Instrument>()
            {
                Instrument.CreateInstrument("Piano"),
                Instrument.CreateInstrument("Flute")
            };
        }
        
        public static MockDbContextBuilder Init() => new MockDbContextBuilder();

        public IMusicTutorDbContext Build() => _dbContext;

        public MockDbContextBuilder WithInstruments()
        {            
            var mock = _instruments.AsQueryable().BuildMockDbSet();
            _dbContext.Instruments.Returns(mock);

            return this;
        }

        public MockDbContextBuilder WithInstruments(Instrument instrument)
        {            
            _instruments.Add(instrument);
            return WithInstruments();
        }
    }
}