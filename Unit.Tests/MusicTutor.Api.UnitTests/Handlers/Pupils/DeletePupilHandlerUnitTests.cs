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
    public class DeletePupilHandlerUnitTests
    {
        private readonly DeletePupilHandler _handler;
        private readonly IMusicTutorDbContext _dbContext;

        private readonly Instrument _instrument;
        private readonly Pupil _Pupil;

        public DeletePupilHandlerUnitTests()
        {
            _instrument = Instrument.CreateInstrument("TEST");
            var instruments = new List<Instrument>();
            instruments.Add(_instrument);
            _Pupil = Pupil.CreatePupil("PupilName", 14M, DateTime.Now, 7, instruments, "ContactName", "ContactEmail", "ContactPhoneNumber");
            _dbContext = MockDbContextBuilder.Init().WithInstruments(_instrument).WithPupils(_Pupil).Build();
            _handler = new DeletePupilHandler(_dbContext);
        }

        [Fact]
        public async Task DeletePupilHandler_DeletesPupilAsync()
        {
            //Given
            var deletePupil = new DeletePupil(_Pupil.Id);
            
            //When
            var response = await _handler.Handle(deletePupil, new CancellationToken());
            
            //Then    
            _dbContext.Pupils.Received().Remove(Arg.Is<Pupil>(_Pupil));            
            await _dbContext.Received().SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task DeletePupilHandler_NotFound_NoDelete()
        {
            //Given
            var guid = Guid.NewGuid();
            var deletePupil = new DeletePupil(guid);
            
            //When
            var response = await _handler.Handle(deletePupil, new CancellationToken());
            
            //Then    
           // response.Should().Be(1);
            _dbContext.Pupils.DidNotReceive().Remove(Arg.Any<Pupil>());
            await _dbContext.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        }
    }
}