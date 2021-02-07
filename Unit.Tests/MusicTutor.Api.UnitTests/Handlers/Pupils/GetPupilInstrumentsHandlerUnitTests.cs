using MapsterMapper;
using MusicTutor.Api.UnitTests.Mapping;
using MusicTutor.Api.EFCore.Handlers.Pupils;
using MusicTutor.Core.Services;
using MusicTutor.Api.UnitTests.Utils;
using Xunit;
using System.Threading.Tasks;
using System.Threading;
using FluentAssertions;
using MusicTutor.Core.Models;
using System.Linq;
using System;
using System.Collections.Generic;
using MusicTutor.Api.Queries.Pupils;

namespace MusicTutor.Api.UnitTests.Handlers.Pupils
{
    public class GetPupilInstrumentsHandlerUnitTests
    {
        private readonly GetPupilInstrumentsHandler _handler;
        private readonly IMusicTutorDbContext _dbContext;

        private readonly Instrument _instrument;
        private readonly Pupil _Pupil;

        public GetPupilInstrumentsHandlerUnitTests()
        {
            _instrument = Instrument.CreateInstrument("TEST");
            var instruments = new List<Instrument>();
            instruments.Add(_instrument);
            _Pupil = Pupil.CreatePupil("PupilName", 14M, DateTime.Now, 7, instruments, "ContactName", "ContactEmail", "ContactPhoneNumber");
            _dbContext = MockDbContextBuilder.Init().WithInstruments(_instrument).WithPupils(_Pupil).Build();
            IMapper mapper = MappingBuilder.Init().Build();
            _handler = new GetPupilInstrumentsHandler(_dbContext, mapper);
        }

        [Fact]
        public async Task GetPupilInstrumentsHandler_GetsInstrumentsAsync()
        {
            //Given
            var getPupilInstruments = new GetPupilInstruments(_Pupil.Id);
            
            //When
            var response = await _handler.Handle(getPupilInstruments, new CancellationToken());
            
            //Then    
            response.Count().Should().Be(1);
        }

        [Fact]
        public async Task GetPupilInstrumentsHandler_NotFound_ReturnsNull()
        {
            //Given
            var guid = Guid.NewGuid();
            var getPupilInstruments = new GetPupilInstruments(guid);
            
            //When
            var response = await _handler.Handle(getPupilInstruments, new CancellationToken());
            
            //Then    
            response.Should().BeNull();
        }
    }
}