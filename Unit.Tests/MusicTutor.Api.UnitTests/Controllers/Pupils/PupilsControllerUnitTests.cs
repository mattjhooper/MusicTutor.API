using MusicTutor.Api.UnitTests.Utils;
using Xunit;
using MusicTutor.Api.Commands.Pupils;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using MusicTutor.Api.Controllers.Pupils;
using MediatR;
using MusicTutor.Api.Contracts.Pupils;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MusicTutor.Api.Queries.Pupils;
using System.Collections.Generic;

namespace MusicTutor.Api.UnitTests.Controllers.Pupils
{
    public class PupilsControllerUnitTests
    {
        PupilResponseDto _pupilDto;
        IMediator _mediator;
        PupilsController _controller;
        CreatePupil _createPupil;
        UpdatePupil _updatePupil;

        public PupilsControllerUnitTests()
        {
            _pupilDto = new PupilResponseDto(Guid.NewGuid(), "Test Name", 10M, DateTime.Now, 7, 0, "Contact Name", "Contact Email", "Contact Phone Number");
            _mediator = Substitute.For<IMediator>();
            _controller = new PupilsController(_mediator);
            _controller.ControllerContext = MockControllerContextBuilder.GetControllerContext();

            _createPupil = new CreatePupil(_pupilDto.Name, _pupilDto.LessonRate, _pupilDto.StartDate, _pupilDto.FrequencyInDays, Guid.NewGuid(), _pupilDto.ContactName, _pupilDto.ContactEmail, _pupilDto.ContactPhoneNumber);
            _updatePupil = new UpdatePupil(_pupilDto.Id, _pupilDto.Name, _pupilDto.LessonRate, _pupilDto.StartDate, _pupilDto.FrequencyInDays, _pupilDto.ContactName, _pupilDto.ContactEmail, _pupilDto.ContactPhoneNumber);

        }

        [Fact]
        public async Task GetManyAsync_ReturnsOkObjectResultAsync()
        {
            // Arrange
            _mediator.Send(Arg.Any<GetAllPupils>()).Returns(new PupilResponseDto[] { _pupilDto });

            // Act
            var response = await _controller.GetManyAsync();

            // Assert
            response.Should().BeOfType<ActionResult<IEnumerable<PupilResponseDto>>>();
            response.Result.Should().BeOfType<OkObjectResult>();
            OkObjectResult result = (OkObjectResult)response.Result;
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task GetSingleAsync_ReturnsOkObjectResultAsync()
        {
            // Arrange
            _mediator.Send(Arg.Any<GetPupilById>()).Returns(_pupilDto);

            // Act
            var response = await _controller.GetSingleAsync(_pupilDto.Id);

            // Assert
            response.Should().BeOfType<ActionResult<PupilResponseDto>>();
            response.Result.Should().BeOfType<OkObjectResult>();
            OkObjectResult result = (OkObjectResult)response.Result;
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task GetSingleAsync_ReturnsNotFoundAsync()
        {
            // Arrange
            _mediator.Send(Arg.Any<GetPupilById>()).Returns<PupilResponseDto>(x => (PupilResponseDto)null);

            // Act
            var response = await _controller.GetSingleAsync(_pupilDto.Id);

            // Assert
            response.Should().BeOfType<ActionResult<PupilResponseDto>>();
            response.Result.Should().BeOfType<NotFoundResult>();
            NotFoundResult result = (NotFoundResult)response.Result;
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact(Skip = "Doing stuff with MusicTutorUser")]
        public async Task PostAsync_ReturnsActionResultPupilResponseDto()
        {
            // Arrange
            _mediator.Send(Arg.Any<CreatePupil>()).Returns(_pupilDto);

            // Act
            var response = await _controller.PostAsync(_createPupil);

            // Assert
            response.Should().BeOfType<ActionResult<PupilResponseDto>>();
            response.Result.Should().BeOfType<CreatedAtRouteResult>();
            CreatedAtRouteResult result = (CreatedAtRouteResult)response.Result;
            result.StatusCode.Should().Be(StatusCodes.Status201Created);
            result.Value.Should().BeOfType<PupilResponseDto>();
            var val = (PupilResponseDto)result.Value;
            val.Name.Should().Be(_pupilDto.Name);
        }

        [Fact(Skip = "Doing stuff with MusicTutorUser")]
        public async Task PostAsync_DbErrorReturnsBadRequest()
        {
            // Arrange
            var dbException = new DbUpdateException("A db error occurred", new InvalidOperationException("An invalid operation occurred"));
            _mediator.Send(Arg.Any<CreatePupil>()).Returns<PupilResponseDto>(x => { throw dbException; });

            // Act
            var response = await _controller.PostAsync(_createPupil);

            // Assert
            response.Should().BeOfType<ActionResult<PupilResponseDto>>();
            response.Result.Should().BeOfType<BadRequestObjectResult>();
            BadRequestObjectResult result = (BadRequestObjectResult)response.Result;
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Value.Should().Be("An invalid operation occurred");
        }

        [Fact]
        public async Task PutSingleAsync_ReturnsOk()
        {
            // Arrange
            _mediator.Send(Arg.Any<UpdatePupil>()).Returns(_pupilDto);

            // Act
            var response = await _controller.PutSingleAsync(_updatePupil.Id, _updatePupil);

            // Assert
            response.Should().BeOfType<ActionResult<PupilResponseDto>>();
            response.Result.Should().BeOfType<OkObjectResult>();
            OkObjectResult result = (OkObjectResult)response.Result;
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeOfType<PupilResponseDto>();
            var val = (PupilResponseDto)result.Value;
            val.Name.Should().Be(_pupilDto.Name);
        }

        [Fact]
        public async Task PutSingleAsync_ReturnsBadRequestIfIdsDoNotMatch()
        {
            // Arrange
            _mediator.Send(Arg.Any<UpdatePupil>()).Returns(_pupilDto);

            // Act
            var response = await _controller.PutSingleAsync(Guid.NewGuid(), _updatePupil);

            // Assert
            response.Should().BeOfType<ActionResult<PupilResponseDto>>();
            response.Result.Should().BeOfType<BadRequestObjectResult>();
            BadRequestObjectResult result = (BadRequestObjectResult)response.Result;
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            ((string)result.Value).Should().Match("Route Id * must match message Body Id *.");
        }

        [Fact]
        public async Task PutSingleAsync_DbErrorReturnsBadRequest()
        {
            // Arrange
            var dbException = new DbUpdateException("A db error occurred", new InvalidOperationException("An invalid operation occurred"));
            _mediator.Send(Arg.Any<UpdatePupil>()).Returns<PupilResponseDto>(x => { throw dbException; });

            // Act
            var response = await _controller.PutSingleAsync(_updatePupil.Id, _updatePupil);

            // Assert
            response.Should().BeOfType<ActionResult<PupilResponseDto>>();
            response.Result.Should().BeOfType<BadRequestObjectResult>();
            BadRequestObjectResult result = (BadRequestObjectResult)response.Result;
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Value.Should().Be("An invalid operation occurred");
        }

        [Fact]
        public async Task PutSingleAsync_InvalidOperationReturnsBadRequest()
        {
            // Arrange
            var invalidOperationException = new InvalidOperationException("An invalid operation occurred");
            _mediator.Send(Arg.Any<UpdatePupil>()).Returns<PupilResponseDto>(x => { throw invalidOperationException; });

            // Act
            var response = await _controller.PutSingleAsync(_updatePupil.Id, _updatePupil);

            // Assert
            response.Should().BeOfType<ActionResult<PupilResponseDto>>();
            response.Result.Should().BeOfType<BadRequestObjectResult>();
            BadRequestObjectResult result = (BadRequestObjectResult)response.Result;
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Value.Should().Be("An invalid operation occurred");
        }

        [Fact]
        public async Task PutSingleAsync_ReturnsNotFoundAsync()
        {
            // Arrange
            PupilResponseDto nullDto = null;
            _mediator.Send(Arg.Any<UpdatePupil>()).Returns<PupilResponseDto>(nullDto);

            // Act
            var response = await _controller.PutSingleAsync(_updatePupil.Id, _updatePupil);

            // Assert
            response.Should().BeOfType<ActionResult<PupilResponseDto>>();
            response.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DeleteItemAsync_ReturnsNoContentAsync()
        {
            // Arrange
            _mediator.Send(Arg.Any<DeletePupil>()).Returns<int>(1);

            // Act
            var response = await _controller.DeleteItemAsync(_pupilDto.Id);

            // Assert
            response.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteItemAsync_ReturnsNotFoundAsync()
        {
            // Arrange
            _mediator.Send(Arg.Any<DeletePupil>()).Returns<int>(0);

            // Act
            var response = await _controller.DeleteItemAsync(_pupilDto.Id);

            // Assert
            response.Should().BeOfType<NotFoundResult>();
        }
    }
}