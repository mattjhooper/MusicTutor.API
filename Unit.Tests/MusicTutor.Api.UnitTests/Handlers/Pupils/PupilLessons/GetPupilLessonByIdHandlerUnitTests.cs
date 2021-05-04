using MusicTutor.Api.EFCore.Handlers.Pupils;
using Xunit;
using System.Threading.Tasks;
using System.Threading;
using FluentAssertions;
using System.Linq;
using System;
using MusicTutor.Api.Queries.Pupils;
using MusicTutor.Api.Contracts.Lessons;
using MusicTutor.Api.Commands.Auth;

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
            var req = new WithMusicTutorUserId<GetPupilLessonById, LessonResponseDto>(_currentUser.Id, _getPupilLessonById);
            //When
            var response = await _handler.Handle(req, new CancellationToken());

            //Then    
            response.Id.Should().Be(_getPupilLessonById.LessonId);
        }

        [Fact]
        public async Task GetPupilLessonByIdHandler_PupilNotFound_ReturnsNull()
        {
            //Given
            var unknownPupil = _getPupilLessonById with { PupilId = Guid.NewGuid() };
            var req = new WithMusicTutorUserId<GetPupilLessonById, LessonResponseDto>(_currentUser.Id, unknownPupil);

            //When
            var response = await _handler.Handle(req, new CancellationToken());

            //Then    
            response.Should().BeNull();
        }

        [Fact]
        public async Task GetPupilLessonByIdHandler_LessonNotFound_ReturnsNull()
        {
            //Given
            var unknownLesson = _getPupilLessonById with { LessonId = Guid.NewGuid() };
            var req = new WithMusicTutorUserId<GetPupilLessonById, LessonResponseDto>(_currentUser.Id, unknownLesson);

            //When
            var response = await _handler.Handle(req, new CancellationToken());

            //Then    
            response.Should().BeNull();
        }
    }
}