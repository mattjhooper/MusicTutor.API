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
using MusicTutor.Api.Contracts.Instruments;

namespace MusicTutor.Api.UnitTests.Handlers.Pupils
{
    public class CreatePupilInstrumentLinkHandlerUnitTests : PupilHandlerUnitTest
    {
        private readonly CreatePupilInstrumentLinkHandler _handler;
        private readonly CreatePupilInstrumentLink _createPupilInstrumentLink;
        private readonly Instrument _newInstrument;

        public CreatePupilInstrumentLinkHandlerUnitTests()
        {
            _handler = new CreatePupilInstrumentLinkHandler(_dbContext, _mapper);
            _newInstrument = _dbContext.Instruments.SingleOrDefault<Instrument>(i => i.Name == "Flute");
            _createPupilInstrumentLink = new CreatePupilInstrumentLink(_pupil.Id, _newInstrument.Id);

        }

        [Fact]
        public async Task CreatePupilInstrumentLinkHandler_AddsInstrumentLinkAsync()
        {
            //Given
            var req = new WithMusicTutorUserId<CreatePupilInstrumentLink, InstrumentResponseDto>(_currentUser.Id, _createPupilInstrumentLink);
            //When
            var response = await _handler.Handle(req, new CancellationToken());

            //Then    
            response.Name.Should().Be(_newInstrument.Name);
            await _dbContext.Received().SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task CreatePupilInstrumentLinkHandler_ReturnsNullForUnknownPupilAsync()
        {
            //Given
            var unknownPupilLink = _createPupilInstrumentLink with { pupilId = Guid.NewGuid() };
            var req = new WithMusicTutorUserId<CreatePupilInstrumentLink, InstrumentResponseDto>(_currentUser.Id, unknownPupilLink);

            //When
            var response = await _handler.Handle(req, new CancellationToken());
            response.Should().BeNull();

            //Then    
            await _dbContext.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task CreatePupilInstrumentLinkHandler_ThrowsExceptionForUnknownInstrumentAsync()
        {
            //Given
            var unknownInstrumentLink = _createPupilInstrumentLink with { instrumentId = Guid.NewGuid() };
            var req = new WithMusicTutorUserId<CreatePupilInstrumentLink, InstrumentResponseDto>(_currentUser.Id, unknownInstrumentLink);

            //When
            Func<Task> act = async () => { await _handler.Handle(req, new CancellationToken()); };
            await act.Should().ThrowAsync<InvalidOperationException>();

            //Then    
            await _dbContext.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        }
    }
}