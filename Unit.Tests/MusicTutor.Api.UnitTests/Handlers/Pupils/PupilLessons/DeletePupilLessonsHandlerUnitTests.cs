using MusicTutor.Api.EFCore.Handlers.Pupils;
using Xunit;
using System.Threading.Tasks;
using System.Threading;
using FluentAssertions;
using System.Linq;
using System;
using MusicTutor.Api.Commands.Pupils;
using NSubstitute;
using MusicTutor.Api.Commands.Auth;

namespace MusicTutor.Api.UnitTests.Handlers.Pupils
{
    public class DeletePupilLessonHandlerUnitTests : PupilHandlerUnitTest
    {
        private readonly DeletePupilLessonHandler _handler;
        private readonly DeletePupilLesson _deletePupilLesson;

        public DeletePupilLessonHandlerUnitTests()
        {
            _handler = new DeletePupilLessonHandler(_dbContext);
            _deletePupilLesson = new DeletePupilLesson(_pupil.Id, _pupil.Lessons.First().Id);
        }

        [Fact]
        public async Task DeletePupilLessonHandler_DeletesLessonsAsync()
        {
            //Given
            var req = new WithMusicTutorUserId<DeletePupilLesson, int>(_currentUser.Id, _deletePupilLesson);
            //When
            var response = await _handler.Handle(req, new CancellationToken());

            //Then    
            response.Should().Be(1);
            _pupil.Lessons.Count().Should().Be(0);
            await _dbContext.Received().SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task DeletePupilLessonHandler_PupilNotFound_ReturnsMinus1()
        {
            //Given
            var unknownPupil = _deletePupilLesson with { PupilId = Guid.NewGuid() };
            var req = new WithMusicTutorUserId<DeletePupilLesson, int>(_currentUser.Id, unknownPupil);

            //When
            var response = await _handler.Handle(req, new CancellationToken());

            //Then    
            response.Should().Be(-1);
        }

        [Fact]
        public async Task DeletePupilLessonHandler_LessonNotFound_Returns0()
        {
            //Given
            var unknownLesson = _deletePupilLesson with { LessonId = Guid.NewGuid() };
            var req = new WithMusicTutorUserId<DeletePupilLesson, int>(_currentUser.Id, unknownLesson);

            //When
            var response = await _handler.Handle(req, new CancellationToken());

            //Then    
            response.Should().Be(0);
        }
    }
}