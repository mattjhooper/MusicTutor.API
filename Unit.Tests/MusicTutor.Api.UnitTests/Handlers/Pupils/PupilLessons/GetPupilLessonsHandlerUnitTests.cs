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
using MusicTutor.Api.Contracts.Lessons;

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
            var req = new WithMusicTutorUserId<GetPupilLessons, IEnumerable<LessonResponseDto>>(_currentUser.Id, getPupilLessons);

            //When
            var response = await _handler.Handle(req, new CancellationToken());

            //Then    
            response.Count().Should().Be(1);
        }

        [Fact]
        public async Task GetPupilLessonsHandler_NotFound_ReturnsNull()
        {
            //Given
            var guid = Guid.NewGuid();
            var getPupilLessons = new GetPupilLessons(guid);
            var req = new WithMusicTutorUserId<GetPupilLessons, IEnumerable<LessonResponseDto>>(_currentUser.Id, getPupilLessons);

            //When
            var response = await _handler.Handle(req, new CancellationToken());

            //Then    
            response.Should().BeNull();
        }
    }
}