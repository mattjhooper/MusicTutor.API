using MusicTutor.Api.EFCore.Handlers.Pupils;
using Xunit;
using MusicTutor.Api.Commands.Pupils;
using System.Threading.Tasks;
using System.Threading;
using FluentAssertions;
using NSubstitute;
using System;

namespace MusicTutor.Api.UnitTests.Handlers.Pupils
{
    public class DeletePupilInstrumentLinkHandlerUnitTests : PupilHandlerUnitTest
    {
        private readonly DeletePupilInstrumentLinkHandler _handler;
        private readonly DeletePupilInstrumentLink _deletePupilInstrumentLink;


        public DeletePupilInstrumentLinkHandlerUnitTests()
        {
            _handler = new DeletePupilInstrumentLinkHandler(_dbContext);
            _deletePupilInstrumentLink = new DeletePupilInstrumentLink(_pupil.Id, _secondInstrument.Id);

        }

        [Fact]
        public async Task DeletePupilInstrumentLinkHandler_RemovesInstrumentLinkAsync()
        {
            //When
            var response = await _handler.Handle(_deletePupilInstrumentLink, new CancellationToken());

            //Then    
            response.Should().Be(1);
            await _dbContext.Received().SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task DeletePupilInstrumentLinkHandler_ReturnsMinus1ForUnknownPupilAsync()
        {
            //Given
            var unknownPupilLink = _deletePupilInstrumentLink with { pupilId = Guid.NewGuid() };

            //When
            var response = await _handler.Handle(unknownPupilLink, new CancellationToken());
            response.Should().Be(-1);

            //Then    
            await _dbContext.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task DeletePupilInstrumentLinkHandler_Returns0ForUnknownInstrumentAsync()
        {
            //Given
            var unknownInstrumentLink = _deletePupilInstrumentLink with { instrumentId = Guid.NewGuid() };

            //When
            var response = await _handler.Handle(unknownInstrumentLink, new CancellationToken());
            response.Should().Be(0);

            //Then    
            await _dbContext.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        }
    }
}