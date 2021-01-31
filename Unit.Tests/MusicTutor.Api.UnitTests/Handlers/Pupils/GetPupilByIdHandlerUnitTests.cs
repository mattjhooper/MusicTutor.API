using MapsterMapper;
using MusicTutor.Api.UnitTests.Mapping;
using MusicTutor.Api.EFCore.Handlers.Pupils;
using MusicTutor.Core.Services;
using MusicTutor.Api.UnitTests.Utils;
using Xunit;
using MusicTutor.Api.Queries.Pupils;
using System.Threading.Tasks;
using System.Threading;
using FluentAssertions;
using NSubstitute;
using MusicTutor.Core.Models;
using System.Linq;
using System;
using System.Collections.Generic;

namespace MusicTutor.Api.UnitTests.Handlers.Pupils
{
    public class GetPupilByIdHandlerUnitTests
    {
        private readonly GetPupilByIdHandler _handler;
        private readonly IMusicTutorDbContext _dbContext;

        private readonly Instrument _instrument;

        private readonly Pupil _pupil;

        public GetPupilByIdHandlerUnitTests()
        {
            _instrument = Instrument.CreateInstrument("TEST");
            var instruments = new List<Instrument>();
            instruments.Add(_instrument);
            _pupil = Pupil.CreatePupil("PupilName", 14M, DateTime.Now, 7, instruments, "ContactName", "ContactEmail", "ContactPhoneNumber");
            _dbContext = MockDbContextBuilder.Init().WithInstruments(_instrument).WithPupils(_pupil).Build();
            IMapper mapper = MappingBuilder.Init().Build();
            _handler = new GetPupilByIdHandler(_dbContext, mapper);
        }

        [Fact]
        public async Task GetPupilByIdHandler_ReturnsPupilAsync()
        {
            //Given
            var getPupil = new GetPupilById(_pupil.Id);
            
            //When
            var response = await _handler.Handle(getPupil, new CancellationToken());
            
            //Then    
            response.Name.Should().Be("PupilName");
        }
    }
}