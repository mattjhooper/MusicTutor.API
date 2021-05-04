using MusicTutor.Api.EFCore.Handlers.Pupils;
using Xunit;
using MusicTutor.Api.Commands.Pupils;
using System.Threading.Tasks;
using System.Threading;
using FluentAssertions;
using NSubstitute;
using System;
using MusicTutor.Api.Commands.Auth;

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
            //Given
            var req = new WithMusicTutorUserId<DeletePupilInstrumentLink, int>(_currentUser.Id, _deletePupilInstrumentLink);
            //When
            var response = await _handler.Handle(req, new CancellationToken());

            //Then    
            response.Should().Be(1);
            await _dbContext.Received().SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task DeletePupilInstrumentLinkHandler_ReturnsMinus1ForUnknownPupilAsync()
        {
            //Given
            var unknownPupilLink = _deletePupilInstrumentLink with { pupilId = Guid.NewGuid() };
            var req = new WithMusicTutorUserId<DeletePupilInstrumentLink, int>(_currentUser.Id, unknownPupilLink);

            //When
            var response = await _handler.Handle(req, new CancellationToken());
            response.Should().Be(-1);

            //Then    
            await _dbContext.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task DeletePupilInstrumentLinkHandler_Returns0ForUnknownInstrumentAsync()
        {
            //Given
            var unknownInstrumentLink = _deletePupilInstrumentLink with { instrumentId = Guid.NewGuid() };
            var req = new WithMusicTutorUserId<DeletePupilInstrumentLink, int>(_currentUser.Id, unknownInstrumentLink);

            //When
            var response = await _handler.Handle(req, new CancellationToken());
            response.Should().Be(0);

            //Then    
            await _dbContext.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        }
    }
}