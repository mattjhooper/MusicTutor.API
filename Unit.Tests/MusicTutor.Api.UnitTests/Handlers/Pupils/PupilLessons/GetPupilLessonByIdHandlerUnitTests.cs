using MusicTutor.Api.EFCore.Handlers.Pupils;
using Xunit;
using System.Threading.Tasks;
using System.Threading;
using FluentAssertions;
using MusicTutor.Core.Models;
using System.Linq;
using System;
using MusicTutor.Api.Commands.Pupils;
using NSubstitute;
using MusicTutor.Api.Queries.Pupils;

namespace MusicTutor.Api.UnitTests.Handlers.Pupils
{
    public class GetPupilLessonByIdHandlerUnitTests : PupilHandlerUnitTest
    {
        private readonly GetPupilLessonByIdHandler _handler;
        private readonly GetPupilLessonById _getPupilLessonById;

        public GetPupilLessonByIdHandlerUnitTests()
        {
            _handler = new GetPupilLessonByIdHandler(_dbContext, _mapper);
            _getPupilLessonById = new GetPupilLessonById(_pupil.Id, _pupil.Lessons.First().Id);
        }

        [Fact]
        public async Task GetPupilLessonByIdHandler_GetsLesson()
        {
            //Given
            //When
            var response = await _handler.Handle(_getPupilLessonById, new CancellationToken());

            //Then    
            response.Id.Should().Be(_getPupilLessonById.lessonId);
        }

        [Fact]
        public async Task GetPupilLessonByIdHandler_PupilNotFound_ReturnsNull()
        {
            //Given
            var unknownPupil = _getPupilLessonById with { pupilId = Guid.NewGuid() };

            //When
            var response = await _handler.Handle(unknownPupil, new CancellationToken());

            //Then    
            response.Should().BeNull();
        }

        [Fact]
        public async Task GetPupilLessonByIdHandler_LessonNotFound_ReturnsNull()
        {
            //Given
            var unknownLesson = _getPupilLessonById with { lessonId = Guid.NewGuid() };

            //When
            var response = await _handler.Handle(unknownLesson, new CancellationToken());

            //Then    
            response.Should().BeNull();
        }
    }
}