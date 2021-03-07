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

namespace MusicTutor.Api.UnitTests.Handlers.Pupils
{
    public class CreatePupilLessonHandlerUnitTests : PupilHandlerUnitTest
    {
        private readonly CreatePupilLessonHandler _handler;
        private readonly CreatePupilLesson _createPupilLesson;

        public CreatePupilLessonHandlerUnitTests()
        {
            _handler = new CreatePupilLessonHandler(_dbContext, _mapper);
            _createPupilLesson = new CreatePupilLesson(_pupil.Id, _pupil.StartDate, 60, _pupil.CurrentLessonRate, false);
        }

        [Fact]
        public async Task CreatePupilLessonHandler_CreatesLessonsAsync()
        {
            //Given
            //When
            var response = await _handler.Handle(_createPupilLesson, new CancellationToken());

            //Then    
            response.Status.Should().Be(Lesson.STATUS_COMPLETE);
            //_pupil.Received().AddLesson(Arg.Is<Lesson>(l => l.PupilId == _pupil.Id && l.Cost == _createPupilLesson.Cost));
            _pupil.Lessons.Count().Should().Be(2);
            _pupil.Lessons.Any(l => l.DurationInMinutes == _createPupilLesson.DurationInMinutes);
            await _dbContext.Received().SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task CreatePupilLessonHandler_NotFound_ReturnsNull()
        {
            //Given
            var unknownPupil = _createPupilLesson with { PupilId = Guid.NewGuid() };

            //When
            var response = await _handler.Handle(unknownPupil, new CancellationToken());

            //Then    
            response.Should().BeNull();
        }
    }
}