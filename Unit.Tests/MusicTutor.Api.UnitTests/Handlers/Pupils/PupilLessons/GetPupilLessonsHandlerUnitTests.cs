using MusicTutor.Api.EFCore.Handlers.Pupils;
using Xunit;
using System.Threading.Tasks;
using System.Threading;
using FluentAssertions;
using System.Linq;
using System;
using System.Collections.Generic;
using MusicTutor.Api.Queries.Pupils;
using MusicTutor.Api.Contracts.Lessons;
using MusicTutor.Api.Commands.Auth;

namespace MusicTutor.Api.UnitTests.Handlers.Pupils
{
    public class GetPupilLessonsHandlerUnitTests : PupilHandlerUnitTest
    {
        private readonly GetPupilLessonsHandler _handler;

        public GetPupilLessonsHandlerUnitTests()
        {
            _handler = new GetPupilLessonsHandler(_dbContext, _mapper);
        }

        [Fact]
        public async Task GetPupilLessonsHandler_GetsLessonsAsync()
        {
            //Given
            var getPupilLessons = new GetPupilLessons(_pupil.Id);

            //When
            var response = await _handler.Handle(getPupilLessons, new CancellationToken());

            //Then    
            response.Count().Should().Be(1);
        }

        [Fact]
        public async Task GetPupilLessonsHandler_NotFound_ReturnsNull()
        {
            //Given
            var guid = Guid.NewGuid();
            var getPupilLessons = new GetPupilLessons(guid);

            //When
            var response = await _handler.Handle(getPupilLessons, new CancellationToken());

            //Then    
            response.Should().BeNull();
        }
    }
}