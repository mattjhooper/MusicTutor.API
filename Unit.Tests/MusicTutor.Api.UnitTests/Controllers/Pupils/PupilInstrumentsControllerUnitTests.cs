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

namespace MusicTutor.Api.UnitTests.Controllers.Pupils
{
    public class PupilInstrumentsControllerUnitTests
    {
        InstrumentResponseDto _instrumentDto;
        IMediator _mediator;

        public PupilInstrumentsControllerUnitTests()
        {
            _instrumentDto = new InstrumentResponseDto(Guid.NewGuid(), "Test Name");
            _mediator = Substitute.For<IMediator>();
            
        }

        [Fact]
        public async Task GetManyAsync_ReturnsOkObjectResultAsync()
        {            
            // Arrange
            _mediator.Send(Arg.Any<GetPupilInstruments>()).Returns(new InstrumentResponseDto[] { _instrumentDto });
            var pupilInstrumentsController = new PupilInstrumentsController(_mediator);
            
            // Act
            var response = await pupilInstrumentsController.GetManyAsync(Guid.NewGuid());

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
            //_mediator.Send(Arg.Any<GetPupilById>()).Returns<PupilResponseDto>(x => (PupilResponseDto)null);
            // Arrange
            _mediator.Send(Arg.Any<GetPupilInstruments>()).Returns<IEnumerable<InstrumentResponseDto>>(x => (IEnumerable<InstrumentResponseDto>)null);
            var pupilInstrumentsController = new PupilInstrumentsController(_mediator);
            
            // Act
            var response = await pupilInstrumentsController.GetManyAsync(Guid.NewGuid());

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
            _mediator.Send(Arg.Any<CreatePupilInstrumentLink>()).Returns(_instrumentDto);
            var pupilInstrumentsController = new PupilInstrumentsController(_mediator);
            
            var createPupilInstrumentLink = new CreatePupilInstrumentLink(Guid.NewGuid(), _instrumentDto.Id);

            // Act
            var response = await pupilInstrumentsController.PostAsync(Guid.NewGuid(), createPupilInstrumentLink);

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
            _mediator.Send(Arg.Any<CreatePupilInstrumentLink>()).Returns((InstrumentResponseDto)null);
            var pupilInstrumentsController = new PupilInstrumentsController(_mediator);
            
            var createPupilInstrumentLink = new CreatePupilInstrumentLink(Guid.NewGuid(), _instrumentDto.Id);

            // Act
            var response = await pupilInstrumentsController.PostAsync(createPupilInstrumentLink.pupilId, createPupilInstrumentLink);

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
            _mediator.Send(Arg.Any<CreatePupilInstrumentLink>()).Returns<InstrumentResponseDto>(x => { throw dbException; });
            var pupilInstrumentsController = new PupilInstrumentsController(_mediator);
            
            var createPupilInstrumentLink = new CreatePupilInstrumentLink(Guid.NewGuid(), _instrumentDto.Id);

            // Act
            var response = await pupilInstrumentsController.PostAsync(createPupilInstrumentLink.pupilId, createPupilInstrumentLink);

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
            _mediator.Send(Arg.Any<CreatePupilInstrumentLink>()).Returns(_instrumentDto);
            var pupilInstrumentsController = new PupilInstrumentsController(_mediator);
            var createPupilInstrumentLink = new CreatePupilInstrumentLink(Guid.NewGuid(), _instrumentDto.Id);

            // Act
            var response = await pupilInstrumentsController.PostAsync(createPupilInstrumentLink.pupilId, createPupilInstrumentLink);

            // Assert
            response.Should().BeOfType<ActionResult<InstrumentResponseDto>>();
            response.Result.Should().BeOfType<CreatedAtRouteResult>();
            CreatedAtRouteResult result = (CreatedAtRouteResult)response.Result;
            result.StatusCode.Should().Be(StatusCodes.Status201Created);
            result.Value.Should().BeOfType<InstrumentResponseDto>();
            var val = (InstrumentResponseDto)result.Value;
            val.Name.Should().Be(_instrumentDto.Name);
        }
    }
}