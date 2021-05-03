using Xunit;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using MusicTutor.Api.Controllers.Pupils;
using MediatR;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MusicTutor.Api.Queries.Pupils;
using System.Collections.Generic;
using MusicTutor.Api.Contracts.Lessons;
using MusicTutor.Api.Commands.Pupils;
using Microsoft.EntityFrameworkCore;
using MusicTutor.Core.Models;
using MusicTutor.Api.UnitTests.Utils;

namespace MusicTutor.Api.UnitTests.Controllers.Pupils
{
    public class PupilLessonsControllerUnitTests
    {
        private readonly LessonResponseDto _lessonDto;
        private readonly IMediator _mediator;
        private readonly PupilLessonsController _controller;
        private readonly CreatePupilLesson _createPupilLesson;


        public PupilLessonsControllerUnitTests()
        {
            _lessonDto = new LessonResponseDto(Guid.NewGuid(), DateTime.Now, 30, 15M, false, Lesson.STATUS_COMPLETE, Guid.NewGuid(), "Main Instrument");
            _mediator = Substitute.For<IMediator>();
            _controller = new PupilLessonsController(_mediator);
            _controller.ControllerContext = MockControllerContextBuilder.GetControllerContext();
            _createPupilLesson = new CreatePupilLesson(Guid.NewGuid(), _lessonDto.StartDateTime, _lessonDto.DurationInMinutes, _lessonDto.Cost, _lessonDto.IsPlanned);
        }

        [Fact]
        public async Task GetSingleAsync_ReturnsOkObjectResultAsync()
        {
            // Arrange
            _mediator.Send(Arg.Any<WithMusicTutorUserId<GetPupilLessonById, LessonResponseDto>>()).Returns(_lessonDto);

            // Act
            var response = await _controller.GetSingleAsync(Guid.NewGuid(), Guid.NewGuid());

            // Assert
            response.Should().BeOfType<ActionResult<LessonResponseDto>>();
            response.Result.Should().BeOfType<OkObjectResult>();
            OkObjectResult result = (OkObjectResult)response.Result;
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task GetSingleAsync_ReturnsNotFoundResultAsync()
        {
            // Arrange
            _mediator.Send(Arg.Any<WithMusicTutorUserId<GetPupilLessonById, LessonResponseDto>>()).Returns((LessonResponseDto)null);

            // Act
            var response = await _controller.GetSingleAsync(Guid.NewGuid(), Guid.NewGuid());

            // Assert
            response.Should().BeOfType<ActionResult<LessonResponseDto>>();
            response.Result.Should().BeOfType<NotFoundResult>();
            NotFoundResult result = (NotFoundResult)response.Result;
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task GetManyAsync_ReturnsOkObjectResultAsync()
        {
            // Arrange
            _mediator.Send(Arg.Any<WithMusicTutorUserId<GetPupilLessons, IEnumerable<LessonResponseDto>>>()).Returns(new LessonResponseDto[] { _lessonDto });

            // Act
            var response = await _controller.GetManyAsync(Guid.NewGuid());

            // Assert
            response.Should().BeOfType<ActionResult<IEnumerable<LessonResponseDto>>>();
            response.Result.Should().BeOfType<OkObjectResult>();
            OkObjectResult result = (OkObjectResult)response.Result;
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task GetManyAsync_EmptyListReturnsOKObject()
        {
            // Arrange
            _mediator.Send(Arg.Any<WithMusicTutorUserId<GetPupilLessons, IEnumerable<LessonResponseDto>>>()).Returns<IEnumerable<LessonResponseDto>>(x => (IEnumerable<LessonResponseDto>)null);

            // Act
            var response = await _controller.GetManyAsync(Guid.NewGuid());

            // Assert
            response.Should().BeOfType<ActionResult<IEnumerable<LessonResponseDto>>>();
            response.Result.Should().BeOfType<OkObjectResult>();
            OkObjectResult result = (OkObjectResult)response.Result;
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task PostAsync_ReturnsBadRequestIfIdsDoNotMatch()
        {
            // Arrange
            _mediator.Send(Arg.Any<WithMusicTutorUserId<CreatePupilLesson, LessonResponseDto>>()).Returns(_lessonDto);

            // Act
            var response = await _controller.PostAsync(Guid.NewGuid(), _createPupilLesson);

            // Assert
            response.Should().BeOfType<ActionResult<LessonResponseDto>>();
            response.Result.Should().BeOfType<BadRequestObjectResult>();
            BadRequestObjectResult result = (BadRequestObjectResult)response.Result;
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            ((string)result.Value).Should().Match("Route Id * must match message Body Id *.");
        }

        [Fact]
        public async Task PostAsync_ReturnsNotFoundAsync()
        {
            // Arrange
            _mediator.Send(Arg.Any<WithMusicTutorUserId<CreatePupilLesson, LessonResponseDto>>()).Returns((LessonResponseDto)null);

            // Act
            var response = await _controller.PostAsync(_createPupilLesson.PupilId, _createPupilLesson);

            // Assert
            response.Should().BeOfType<ActionResult<LessonResponseDto>>();
            response.Result.Should().BeOfType<NotFoundResult>();
            NotFoundResult result = (NotFoundResult)response.Result;
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task PostAsync_DbErrorReturnsBadRequest()
        {
            // Arrange
            var dbException = new DbUpdateException("A db error occurred", new InvalidOperationException("An invalid operation occurred"));
            _mediator.Send(Arg.Any<WithMusicTutorUserId<CreatePupilLesson, LessonResponseDto>>()).Returns<LessonResponseDto>(x => { throw dbException; });

            // Act
            var response = await _controller.PostAsync(_createPupilLesson.PupilId, _createPupilLesson);

            // Assert
            response.Should().BeOfType<ActionResult<LessonResponseDto>>();
            response.Result.Should().BeOfType<BadRequestObjectResult>();
            BadRequestObjectResult result = (BadRequestObjectResult)response.Result;
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Value.Should().Be("An invalid operation occurred");
        }

        [Fact]
        public async Task PostAsync_ReturnsActionResultLessonResponseDto()
        {
            // Arrange
            _mediator.Send(Arg.Any<WithMusicTutorUserId<CreatePupilLesson, LessonResponseDto>>()).Returns(_lessonDto);

            // Act
            var response = await _controller.PostAsync(_createPupilLesson.PupilId, _createPupilLesson);

            // Assert
            response.Should().BeOfType<ActionResult<LessonResponseDto>>();
            response.Result.Should().BeOfType<CreatedAtRouteResult>();
            CreatedAtRouteResult result = (CreatedAtRouteResult)response.Result;
            result.StatusCode.Should().Be(StatusCodes.Status201Created);
            result.Value.Should().BeOfType<LessonResponseDto>();
            var val = (LessonResponseDto)result.Value;
            val.InstrumentName.Should().Be(_lessonDto.InstrumentName);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsNotFoundAsync()
        {
            // Arrange
            _mediator.Send(Arg.Any<WithMusicTutorUserId<DeletePupilLesson, int>>()).Returns(-1);

            // Act
            var response = await _controller.DeleteAsync(Guid.NewGuid(), _lessonDto.Id);

            // Assert
            response.Should().BeOfType<NotFoundResult>();
            NotFoundResult result = (NotFoundResult)response;
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task DeleteAsync_DbErrorReturnsBadRequest()
        {
            // Arrange
            var dbException = new DbUpdateException("A db error occurred", new InvalidOperationException("An invalid operation occurred"));
            _mediator.Send(Arg.Any<WithMusicTutorUserId<DeletePupilLesson, int>>()).Returns<int>(x => { throw dbException; });

            // Act
            var response = await _controller.DeleteAsync(Guid.NewGuid(), _lessonDto.Id);

            // Assert
            response.Should().BeOfType<BadRequestObjectResult>();
            BadRequestObjectResult result = (BadRequestObjectResult)response;
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Value.Should().Be("An invalid operation occurred");
        }

        [Fact]
        public async Task DeleteItemAsync_ReturnsNoContentAsync()
        {
            // Arrange
            _mediator.Send(Arg.Any<WithMusicTutorUserId<DeletePupilLesson, int>>()).Returns<int>(1);

            // Act
            var response = await _controller.DeleteAsync(Guid.NewGuid(), _lessonDto.Id);

            // Assert
            response.Should().BeOfType<NoContentResult>();
        }
    }
}