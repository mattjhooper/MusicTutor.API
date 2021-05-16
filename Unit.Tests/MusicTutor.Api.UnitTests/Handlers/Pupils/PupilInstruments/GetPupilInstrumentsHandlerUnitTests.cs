using MusicTutor.Api.EFCore.Handlers.Pupils;
using Xunit;
using System.Threading.Tasks;
using System.Threading;
using FluentAssertions;
using System.Linq;
using System;
using System.Collections.Generic;
using MusicTutor.Api.Queries.Pupils;
using MusicTutor.Api.Contracts.Instruments;
using MusicTutor.Api.Commands.Auth;

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

            //When
            var response = await _handler.Handle(getPupilInstruments, new CancellationToken());

            //Then    
            response.Count().Should().Be(2);
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