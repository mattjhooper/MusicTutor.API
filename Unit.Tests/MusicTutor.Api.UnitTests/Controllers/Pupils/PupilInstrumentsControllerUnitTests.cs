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
    }
}