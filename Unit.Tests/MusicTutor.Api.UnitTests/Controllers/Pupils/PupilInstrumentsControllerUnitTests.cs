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
using MusicTutor.Api.Contracts.Instruments;
using MusicTutor.Api.Commands.Pupils;
using Microsoft.EntityFrameworkCore;
using FluentValidation.Results;
using MusicTutor.Api.UnitTests.Utils;
using MusicTutor.Api.Commands.Auth;

namespace MusicTutor.Api.UnitTests.Controllers.Pupils
{
    public class PupilInstrumentsControllerUnitTests
    {
        private readonly InstrumentResponseDto _instrumentDto;
        private readonly IMediator _mediator;
        private readonly PupilInstrumentsController _controller;


        public PupilInstrumentsControllerUnitTests()
        {
            _instrumentDto = new InstrumentResponseDto(Guid.NewGuid(), "Test Name");
            _mediator = Substitute.For<IMediator>();
            _controller = new PupilInstrumentsController(_mediator);
            _controller.ControllerContext = MockControllerContextBuilder.GetControllerContext();
        }

        [Fact]
        public async Task GetManyAsync_ReturnsOkObjectResultAsync()
        {
            // Arrange
            _mediator.Send(Arg.Any<WithMusicTutorUserId<GetPupilInstruments, IEnumerable<InstrumentResponseDto>>>()).Returns(new InstrumentResponseDto[] { _instrumentDto });

            // Act
            var response = await _controller.GetManyAsync(Guid.NewGuid());

            // Assert
            response.Should().BeOfType<ActionResult<IEnumerable<InstrumentResponseDto>>>();
            response.Result.Should().BeOfType<OkObjectResult>();
            OkObjectResult result = (OkObjectResult)response.Result;
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task GetManyAsync_ReturnsNotFoundAsync()
        {
            // Arrange
            _mediator.Send(Arg.Any<WithMusicTutorUserId<GetPupilInstruments, IEnumerable<InstrumentResponseDto>>>()).Returns<IEnumerable<InstrumentResponseDto>>(x => (IEnumerable<InstrumentResponseDto>)null);

            // Act
            var response = await _controller.GetManyAsync(Guid.NewGuid());

            // Assert
            response.Should().BeOfType<ActionResult<IEnumerable<InstrumentResponseDto>>>();
            response.Result.Should().BeOfType<NotFoundResult>();
            NotFoundResult result = (NotFoundResult)response.Result;
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task PostAsync_ReturnsBadRequestIfIdsDoNotMatch()
        {
            // Arrange
            _mediator.Send(Arg.Any<WithMusicTutorUserId<CreatePupilInstrumentLink, InstrumentResponseDto>>()).Returns(_instrumentDto);

            var createPupilInstrumentLink = new CreatePupilInstrumentLink(Guid.NewGuid(), _instrumentDto.Id);

            // Act
            var response = await _controller.PostAsync(Guid.NewGuid(), createPupilInstrumentLink);

            // Assert
            response.Should().BeOfType<ActionResult<InstrumentResponseDto>>();
            response.Result.Should().BeOfType<BadRequestObjectResult>();
            BadRequestObjectResult result = (BadRequestObjectResult)response.Result;
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            ((string)result.Value).Should().Match("Route Id * must match message Body Id *.");
        }

        [Fact]
        public async Task PostAsync_ReturnsNotFoundAsync()
        {
            // Arrange
            _mediator.Send(Arg.Any<WithMusicTutorUserId<CreatePupilInstrumentLink, InstrumentResponseDto>>()).Returns((InstrumentResponseDto)null);

            var createPupilInstrumentLink = new CreatePupilInstrumentLink(Guid.NewGuid(), _instrumentDto.Id);

            // Act
            var response = await _controller.PostAsync(createPupilInstrumentLink.pupilId, createPupilInstrumentLink);

            // Assert
            response.Should().BeOfType<ActionResult<InstrumentResponseDto>>();
            response.Result.Should().BeOfType<NotFoundResult>();
            NotFoundResult result = (NotFoundResult)response.Result;
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task PostAsync_DbErrorReturnsBadRequest()
        {
            // Arrange
            var dbException = new DbUpdateException("A db error occurred", new InvalidOperationException("An invalid operation occurred"));
            _mediator.Send(Arg.Any<WithMusicTutorUserId<CreatePupilInstrumentLink, InstrumentResponseDto>>()).Returns<InstrumentResponseDto>(x => { throw dbException; });

            var createPupilInstrumentLink = new CreatePupilInstrumentLink(Guid.NewGuid(), _instrumentDto.Id);

            // Act
            var response = await _controller.PostAsync(createPupilInstrumentLink.pupilId, createPupilInstrumentLink);

            // Assert
            response.Should().BeOfType<ActionResult<InstrumentResponseDto>>();
            response.Result.Should().BeOfType<BadRequestObjectResult>();
            BadRequestObjectResult result = (BadRequestObjectResult)response.Result;
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Value.Should().Be("An invalid operation occurred");
        }

        [Fact]
        public async Task PostAsync_ReturnsActionResultInstrumentResponseDto()
        {
            // Arrange
            _mediator.Send(Arg.Any<WithMusicTutorUserId<CreatePupilInstrumentLink, InstrumentResponseDto>>()).Returns(_instrumentDto);
            var createPupilInstrumentLink = new CreatePupilInstrumentLink(Guid.NewGuid(), _instrumentDto.Id);

            // Act
            var response = await _controller.PostAsync(createPupilInstrumentLink.pupilId, createPupilInstrumentLink);

            // Assert
            response.Should().BeOfType<ActionResult<InstrumentResponseDto>>();
            response.Result.Should().BeOfType<CreatedAtRouteResult>();
            CreatedAtRouteResult result = (CreatedAtRouteResult)response.Result;
            result.StatusCode.Should().Be(StatusCodes.Status201Created);
            result.Value.Should().BeOfType<InstrumentResponseDto>();
            var val = (InstrumentResponseDto)result.Value;
            val.Name.Should().Be(_instrumentDto.Name);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsNotFoundAsync()
        {
            // Arrange
            _mediator.Send(Arg.Any<WithMusicTutorUserId<DeletePupilInstrumentLink, int>>()).Returns(-1);

            // Act
            var response = await _controller.DeleteAsync(Guid.NewGuid(), _instrumentDto.Id);

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
            _mediator.Send(Arg.Any<WithMusicTutorUserId<DeletePupilInstrumentLink, int>>()).Returns<int>(x => { throw dbException; });

            // Act
            var response = await _controller.DeleteAsync(Guid.NewGuid(), _instrumentDto.Id);

            // Assert
            response.Should().BeOfType<BadRequestObjectResult>();
            BadRequestObjectResult result = (BadRequestObjectResult)response;
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Value.Should().Be("An invalid operation occurred");
        }

        [Fact]
        public async Task DeleteAsync_FluentValidationErrorReturnsBadRequest()
        {
            // Arrange
            var errors = new List<ValidationFailure>
            {
                new ValidationFailure("Fieldname", "Error Message")
            };
            var fluentValidationException = new FluentValidation.ValidationException(errors);

            _mediator.Send(Arg.Any<WithMusicTutorUserId<DeletePupilInstrumentLink, int>>()).Returns<int>(x => { throw fluentValidationException; });

            // Act
            var response = await _controller.DeleteAsync(Guid.NewGuid(), _instrumentDto.Id);

            // Assert
            response.Should().BeOfType<BadRequestObjectResult>();
            BadRequestObjectResult result = (BadRequestObjectResult)response;
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Value.Should().BeOfType<List<ValidationFailure>>();
            List<ValidationFailure> validationErrors = (List<ValidationFailure>)result.Value;
            validationErrors[0].ErrorMessage.Should().Be("Error Message");
        }

        [Fact]
        public async Task DeleteItemAsync_ReturnsNoContentAsync()
        {
            // Arrange
            _mediator.Send(Arg.Any<WithMusicTutorUserId<DeletePupilInstrumentLink, int>>()).Returns<int>(1);

            // Act
            var response = await _controller.DeleteAsync(Guid.NewGuid(), _instrumentDto.Id);

            // Assert
            response.Should().BeOfType<NoContentResult>();
        }
    }
}