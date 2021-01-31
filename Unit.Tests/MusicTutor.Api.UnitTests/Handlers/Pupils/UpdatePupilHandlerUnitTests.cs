using MapsterMapper;
using MusicTutor.Api.UnitTests.Mapping;
using MusicTutor.Api.EFCore.Handlers.Pupils;
using MusicTutor.Core.Services;
using MusicTutor.Api.UnitTests.Utils;
using Xunit;
using MusicTutor.Api.Commands.Pupils;
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
    public class UpdatePupilHandlerUnitTests
    {
        private readonly UpdatePupilHandler _handler;
        private readonly IMusicTutorDbContext _dbContext;

        private readonly Instrument _instrument;
        private readonly Pupil _Pupil;

        public UpdatePupilHandlerUnitTests()
        {
            _instrument = Instrument.CreateInstrument("TEST");
            var instruments = new List<Instrument>();
            instruments.Add(_instrument);
            _Pupil = Pupil.CreatePupil("PupilName", 14M, DateTime.Now, 7, instruments, "ContactName", "ContactEmail", "ContactPhoneNumber");
            _dbContext = MockDbContextBuilder.Init().WithInstruments(_instrument).WithPupils(_Pupil).Build();
            IMapper mapper = MappingBuilder.Init().Build();
            _handler = new UpdatePupilHandler(_dbContext, mapper);
        }

        [Fact]
        public async Task UpdatePupilHandler_UpdatesPupilAsync()
        {
            //Given
            var updatePupil = new UpdatePupil(_Pupil.Id, "NewName", 15M, _Pupil.StartDate.AddHours(1), 14, "NewContactName", "NewContactEmail", "NewContactPhoneNumber" );
            
            //When
            var response = await _handler.Handle(updatePupil, new CancellationToken());
            
            //Then    
            response.Name.Should().Be(updatePupil.Name);
            response.LessonRate.Should().Be(updatePupil.LessonRate);
            response.StartDate.Should().Be(updatePupil.StartDate);
            response.FrequencyInDays.Should().Be(updatePupil.FrequencyInDays);
            response.ContactName.Should().Be(updatePupil.ContactName);
            response.ContactEmail.Should().Be(updatePupil.ContactEmail);
            response.ContactPhoneNumber.Should().Be(updatePupil.ContactPhoneNumber);
            await _dbContext.Received().SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task CreatePupilHandler_ReturnsNullForUnknownInstrumentAsync()
        {
            //Given
            var updatePupil = new UpdatePupil(Guid.NewGuid(), "NewName", 15M, _Pupil.StartDate.AddHours(1), 14, "NewContactName", "NewContactEmail", "NewContactPhoneNumber" );
            
            //When
            var response = await _handler.Handle(updatePupil, new CancellationToken());
            response.Should().BeNull();
            
            //Then    
            await _dbContext.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        }
    }
}