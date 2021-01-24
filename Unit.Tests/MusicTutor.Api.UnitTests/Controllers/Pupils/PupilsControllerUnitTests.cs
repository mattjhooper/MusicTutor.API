using MapsterMapper;
using MusicTutor.Api.UnitTests.Mapping;
using MusicTutor.Api.EFCore.Handlers.Pupils;
using MusicTutor.Core.Services;
using MusicTutor.Api.UnitTests.Utils;
using Xunit;
using MusicTutor.Api.Commands.Pupils;
using System.Threading.Tasks;
using System.Threading;
using FluentAssertions;
using NSubstitute;
using MusicTutor.Core.Models;
using System.Linq;
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

        public PupilsControllerUnitTests()
        {
            _pupilDto = new PupilResponseDto(Guid.NewGuid(), "Test Name", 10M, DateTime.Now, 7, 0, "Contact Name", "Contact Email", "Contact Phone Number");
            _mediator = Substitute.For<IMediator>();
            
        }

        [Fact]
        public async Task GetManyAsync_ReturnsOkObjectResultAsync()
        {            
            // Arrange
            _mediator.Send(Arg.Any<GetAllPupils>()).Returns(new PupilResponseDto[] { _pupilDto });
            var PupilsController = new PupilsController(_mediator);
            
            // Act
            var response = await PupilsController.GetManyAsync();

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
            var PupilsController = new PupilsController(_mediator);
            
            // Act
            var response = await PupilsController.GetSingleAsync(_pupilDto.Id);

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
            var PupilsController = new PupilsController(_mediator);
            
            // Act
            var response = await PupilsController.GetSingleAsync(_pupilDto.Id);

            // Assert
            response.Should().BeOfType<ActionResult<PupilResponseDto>>();
            response.Result.Should().BeOfType<NotFoundResult>();
            NotFoundResult result = (NotFoundResult)response.Result;
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task PostAsync_ReturnsActionResultPupilResponseDto()
        {            
            // Arrange
            _mediator.Send(Arg.Any<CreatePupil>()).Returns(_pupilDto);
            var PupilsController = new PupilsController(_mediator);
            
            var createPupil = new CreatePupil(_pupilDto.Name, _pupilDto.LessonRate, _pupilDto.StartDate, _pupilDto.FrequencyInDays, Guid.NewGuid(), _pupilDto.ContactName, _pupilDto.ContactEmail, _pupilDto.ContactPhoneNumber);

            // Act
            var response = await PupilsController.PostAsync(createPupil);

            // Assert
            response.Should().BeOfType<ActionResult<PupilResponseDto>>();
            response.Result.Should().BeOfType<CreatedAtRouteResult>();
            CreatedAtRouteResult result = (CreatedAtRouteResult)response.Result;
            result.StatusCode.Should().Be(StatusCodes.Status201Created);
            result.Value.Should().BeOfType<PupilResponseDto>();
            var val = (PupilResponseDto)result.Value;
            val.Name.Should().Be(_pupilDto.Name);
        }

        [Fact]
        public async Task PostAsync_DbErrorReturnsBadRequest()
        {            
            // Arrange
            var dbException = new DbUpdateException("A db error occurred", new InvalidOperationException("An invalid operation occurred"));
            _mediator.Send(Arg.Any<CreatePupil>()).Returns<PupilResponseDto>(x => { throw dbException; });
            var PupilsController = new PupilsController(_mediator);
            
            var createPupil = new CreatePupil(_pupilDto.Name, _pupilDto.LessonRate, _pupilDto.StartDate, _pupilDto.FrequencyInDays, Guid.NewGuid(), _pupilDto.ContactName, _pupilDto.ContactEmail, _pupilDto.ContactPhoneNumber);

            // Act
            var response = await PupilsController.PostAsync(createPupil);

            // Assert
            response.Should().BeOfType<ActionResult<PupilResponseDto>>();
            response.Result.Should().BeOfType<BadRequestObjectResult>();
            BadRequestObjectResult result = (BadRequestObjectResult)response.Result;
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Value.Should().Be("An invalid operation occurred");
        }

        [Fact]
        public async Task PutAsync_ReturnsOk()
        {            
            // Arrange
            _mediator.Send(Arg.Any<UpdatePupil>()).Returns(_pupilDto);
            var PupilsController = new PupilsController(_mediator);
            
            var updatePupil = new UpdatePupil(_pupilDto.Id, _pupilDto.Name, _pupilDto.LessonRate, _pupilDto.StartDate, _pupilDto.FrequencyInDays, _pupilDto.ContactName, _pupilDto.ContactEmail, _pupilDto.ContactPhoneNumber);

            // Act
            var response = await PupilsController.PutAsync(updatePupil);

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
        public async Task PutAsync_DbErrorReturnsBadRequest()
        {            
            // Arrange
            var dbException = new DbUpdateException("A db error occurred", new InvalidOperationException("An invalid operation occurred"));
            _mediator.Send(Arg.Any<UpdatePupil>()).Returns<PupilResponseDto>(x => { throw dbException; });
            var PupilsController = new PupilsController(_mediator);
            
            var updatePupil = new UpdatePupil(_pupilDto.Id, _pupilDto.Name, _pupilDto.LessonRate, _pupilDto.StartDate, _pupilDto.FrequencyInDays, _pupilDto.ContactName, _pupilDto.ContactEmail, _pupilDto.ContactPhoneNumber);

            // Act
            var response = await PupilsController.PutAsync(updatePupil);

            // Assert
            response.Should().BeOfType<ActionResult<PupilResponseDto>>();
            response.Result.Should().BeOfType<BadRequestObjectResult>();
            BadRequestObjectResult result = (BadRequestObjectResult)response.Result;
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Value.Should().Be("An invalid operation occurred");
        }

        [Fact]
        public async Task DeleteItemAsync_ReturnsNoContentAsync()
        {            
            // Arrange
            _mediator.Send(Arg.Any<DeletePupil>()).Returns<int>(1);
            var PupilsController = new PupilsController(_mediator);
            
            // Act
            var response = await PupilsController.DeleteItemAsync(_pupilDto.Id);

            // Assert
            response.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteItemAsync_ReturnsNotFoundAsync()
        {            
            // Arrange
            _mediator.Send(Arg.Any<DeletePupil>()).Returns<int>(0);
            var PupilsController = new PupilsController(_mediator);
            
            // Act
            var response = await PupilsController.DeleteItemAsync(_pupilDto.Id);

            // Assert
            response.Should().BeOfType<NotFoundResult>();
        }          
    }
}