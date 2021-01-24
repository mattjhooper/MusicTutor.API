using MapsterMapper;
using MusicTutor.Api.UnitTests.Mapping;
using MusicTutor.Api.EFCore.Handlers.Instruments;
using MusicTutor.Core.Services;
using MusicTutor.Api.UnitTests.Utils;
using Xunit;
using MusicTutor.Api.Commands.Instruments;
using System.Threading.Tasks;
using System.Threading;
using FluentAssertions;
using NSubstitute;
using MusicTutor.Core.Models;
using System.Linq;
using MusicTutor.Api.Controllers.Instruments;
using MediatR;
using MusicTutor.Api.Contracts.Instruments;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MusicTutor.Api.Queries.Instruments;
using System.Collections.Generic;

namespace MusicTutor.Api.UnitTests.Controllers.Instruments
{
    public class InstrumentsControllerUnitTests
    {
        InstrumentResponseDto _instrumentDto;
        IMediator _mediator;

        public InstrumentsControllerUnitTests()
        {
            _instrumentDto = new InstrumentResponseDto(Guid.NewGuid(), "TEST");
            _mediator = Substitute.For<IMediator>();
            
        }

        [Fact]
        public async Task GetManyAsync_ReturnsOkObjectResultAsync()
        {            
            // Arrange
            _mediator.Send(Arg.Any<GetAllInstruments>()).Returns(new InstrumentResponseDto[] { _instrumentDto });
            var instrumentsController = new InstrumentsController(_mediator);
            
            // Act
            var response = await instrumentsController.GetManyAsync();

            // Assert
            response.Should().BeOfType<ActionResult<IEnumerable<InstrumentResponseDto>>>();
            response.Result.Should().BeOfType<OkObjectResult>();
            OkObjectResult result = (OkObjectResult)response.Result;
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task GetSingleAsync_ReturnsOkObjectResultAsync()
        {            
            // Arrange
            _mediator.Send(Arg.Any<GetInstrumentById>()).Returns(_instrumentDto);
            var instrumentsController = new InstrumentsController(_mediator);
            
            // Act
            var response = await instrumentsController.GetSingleAsync(_instrumentDto.Id);

            // Assert
            response.Should().BeOfType<ActionResult<InstrumentResponseDto>>();
            response.Result.Should().BeOfType<OkObjectResult>();
            OkObjectResult result = (OkObjectResult)response.Result;
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task GetSingleAsync_ReturnsNotFoundAsync()
        {            
            // Arrange
            _mediator.Send(Arg.Any<GetInstrumentById>()).Returns<InstrumentResponseDto>(x => (InstrumentResponseDto)null);
            var instrumentsController = new InstrumentsController(_mediator);
            
            // Act
            var response = await instrumentsController.GetSingleAsync(_instrumentDto.Id);

            // Assert
            response.Should().BeOfType<ActionResult<InstrumentResponseDto>>();
            response.Result.Should().BeOfType<NotFoundResult>();
            NotFoundResult result = (NotFoundResult)response.Result;
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task PostAsync_ReturnsActionResultInstrumentResponseDto()
        {            
            // Arrange
            _mediator.Send(Arg.Any<CreateInstrument>()).Returns(_instrumentDto);
            var instrumentsController = new InstrumentsController(_mediator);
            
            var createInstrument = new CreateInstrument("TEST");

            // Act
            var response = await instrumentsController.PostAsync(createInstrument);

            // Assert
            response.Should().BeOfType<ActionResult<InstrumentResponseDto>>();
            response.Result.Should().BeOfType<CreatedAtRouteResult>();
            CreatedAtRouteResult result = (CreatedAtRouteResult)response.Result;
            result.StatusCode.Should().Be(StatusCodes.Status201Created);
            result.Value.Should().BeOfType<InstrumentResponseDto>();
            var val = (InstrumentResponseDto)result.Value;
            val.Name.Should().Be("TEST");
        }

        [Fact]
        public async Task PostAsync_DbErrorReturnsBadRequest()
        {            
            // Arrange
            var dbException = new DbUpdateException("A db error occurred", new InvalidOperationException("An invalid operation occurred"));
            _mediator.Send(Arg.Any<CreateInstrument>()).Returns<InstrumentResponseDto>(x => { throw dbException; });
            var instrumentsController = new InstrumentsController(_mediator);
            
            var createInstrument = new CreateInstrument("TEST");
            
            // Act
            var response = await instrumentsController.PostAsync(createInstrument);

            // Assert
            response.Should().BeOfType<ActionResult<InstrumentResponseDto>>();
            response.Result.Should().BeOfType<BadRequestObjectResult>();
            BadRequestObjectResult result = (BadRequestObjectResult)response.Result;
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Value.Should().Be("An invalid operation occurred");
        }

        [Fact]
        public async Task DeleteItemAsync_ReturnsNoContentAsync()
        {            
            // Arrange
            _mediator.Send(Arg.Any<DeleteInstrument>()).Returns<int>(1);
            var instrumentsController = new InstrumentsController(_mediator);
            
            // Act
            var response = await instrumentsController.DeleteItemAsync(_instrumentDto.Id);

            // Assert
            response.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteItemAsync_ReturnsNotFoundAsync()
        {            
            // Arrange
            _mediator.Send(Arg.Any<DeleteInstrument>()).Returns<int>(0);
            var instrumentsController = new InstrumentsController(_mediator);
            
            // Act
            var response = await instrumentsController.DeleteItemAsync(_instrumentDto.Id);

            // Assert
            response.Should().BeOfType<NotFoundResult>();
        }          
    }
}