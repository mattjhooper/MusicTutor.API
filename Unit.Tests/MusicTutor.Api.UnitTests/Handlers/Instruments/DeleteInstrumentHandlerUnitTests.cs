using MapsterMapper;
using MusicTutor.Api.UnitTests.Mapping;
using MusicTutor.Api.EFCore.Handlers.Instruments;
using MusicTutor.Core.Services;
using MusicTutor.Api.UnitTests.Utils;
using Xunit;
using MusicTutor.Api.Commands.Instruments;
using System.Threading.Tasks;
using System.Threading;
using FluentAssertions;
using NSubstitute;
using MusicTutor.Core.Models;
using System.Linq;
using System;

namespace MusicTutor.Api.UnitTests.Handlers.Instruments
{
    public class DeleteInstrumentHandlerUnitTests
    {
        private readonly DeleteInstrumentHandler _handler;
        private readonly IMusicTutorDbContext _dbContext;

        private readonly Instrument _instrument;

        public DeleteInstrumentHandlerUnitTests()
        {
            _instrument = Instrument.CreateInstrument("TEST");
            _dbContext = MockDbContextBuilder.Init().WithInstruments(_instrument).Build();
            _handler = new DeleteInstrumentHandler(_dbContext);
        }

        [Fact]
        public async Task DeleteInstrumentHandler_DeletesInstrumentAsync()
        {
            //Given
            var deleteInstrument = new DeleteInstrument(_instrument.Id);
            
            //When
            var response = await _handler.Handle(deleteInstrument, new CancellationToken());
            
            //Then    
            _dbContext.Instruments.Received().Remove(Arg.Is<Instrument>(_instrument));            
            await _dbContext.Received().SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task DeleteInstrumentHandler_NotFound_NoDelete()
        {
            //Given
            var guid = Guid.NewGuid();
            var deleteInstrument = new DeleteInstrument(guid);
            
            //When
            var response = await _handler.Handle(deleteInstrument, new CancellationToken());
            
            //Then    
           // response.Should().Be(1);
            _dbContext.Instruments.DidNotReceive().Remove(Arg.Any<Instrument>());
            await _dbContext.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        }
    }
}