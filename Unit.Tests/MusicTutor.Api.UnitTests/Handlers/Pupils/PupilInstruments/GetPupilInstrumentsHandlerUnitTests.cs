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
using MusicTutor.Api.Commands.Pupils;
using MusicTutor.Api.Contracts.Instruments;

namespace MusicTutor.Api.UnitTests.Handlers.Pupils
{
    public class GetPupilInstrumentsHandlerUnitTests : PupilHandlerUnitTest
    {
        private readonly GetPupilInstrumentsHandler _handler;

        public GetPupilInstrumentsHandlerUnitTests()
        {
            _handler = new GetPupilInstrumentsHandler(_dbContext, _mapper);
        }

        [Fact]
        public async Task GetPupilInstrumentsHandler_GetsInstrumentsAsync()
        {
            //Given
            var getPupilInstruments = new GetPupilInstruments(_pupil.Id);
            var req = new WithMusicTutorUserId<GetPupilInstruments, IEnumerable<InstrumentResponseDto>>(_currentUser.Id, getPupilInstruments);

            //When
            var response = await _handler.Handle(req, new CancellationToken());

            //Then    
            response.Count().Should().Be(2);
        }

        [Fact]
        public async Task GetPupilInstrumentsHandler_NotFound_ReturnsNull()
        {
            //Given
            var guid = Guid.NewGuid();
            var getPupilInstruments = new GetPupilInstruments(guid);
            var req = new WithMusicTutorUserId<GetPupilInstruments, IEnumerable<InstrumentResponseDto>>(_currentUser.Id, getPupilInstruments);

            //When
            var response = await _handler.Handle(req, new CancellationToken());

            //Then    
            response.Should().BeNull();
        }
    }
}