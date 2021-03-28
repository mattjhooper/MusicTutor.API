using System;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicTutor.Api.Commands.Pupils;
using MusicTutor.Api.Contracts.Payments;
using MusicTutor.Api.Controllers.Pupils;
using MusicTutor.Core.Models.Enums;
using NSubstitute;
using Xunit;

namespace MusicTutor.Api.UnitTests.Controllers.Pupils
{
    public class PupilPaymentsControllerUnitTests
    {
        private readonly PaymentResponseDto _paymentDto;
        private readonly IMediator _mediator;
        private readonly PupilPaymentsController _controller;
        private readonly CreatePupilPayment _createPupilPayment;


        public PupilPaymentsControllerUnitTests()
        {
            _paymentDto = new PaymentResponseDto(Guid.NewGuid(), DateTime.Now, 15M, PaymentType.Cash);
            _mediator = Substitute.For<IMediator>();
            _controller = new PupilPaymentsController(_mediator);
            _createPupilPayment = new CreatePupilPayment(Guid.NewGuid(), _paymentDto.PaymentDate, _paymentDto.Amount, _paymentDto.Type);
        }

        [Fact]
        public async Task PostAsync_ReturnsBadRequestIfIdsDoNotMatch()
        {
            // Arrange
            _mediator.Send(Arg.Any<CreatePupilPayment>()).Returns(_paymentDto);

            // Act
            var response = await _controller.PostAsync(Guid.NewGuid(), _createPupilPayment);

            // Assert
            response.Should().BeOfType<ActionResult<PaymentResponseDto>>();
            response.Result.Should().BeOfType<BadRequestObjectResult>();
            BadRequestObjectResult result = (BadRequestObjectResult)response.Result;
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            ((string)result.Value).Should().Match("Route Id * must match message Body Id *.");
        }


        [Fact]
        public async Task PostAsync_ReturnsNotFoundAsync()
        {
            // Arrange
            _mediator.Send(Arg.Any<CreatePupilPayment>()).Returns((PaymentResponseDto)null);

            // Act
            var response = await _controller.PostAsync(_createPupilPayment.PupilId, _createPupilPayment);

            // Assert
            response.Should().BeOfType<ActionResult<PaymentResponseDto>>();
            response.Result.Should().BeOfType<NotFoundResult>();
            NotFoundResult result = (NotFoundResult)response.Result;
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task PostAsync_DbErrorReturnsBadRequest()
        {
            // Arrange
            var dbException = new DbUpdateException("A db error occurred", new InvalidOperationException("An invalid operation occurred"));
            _mediator.Send(Arg.Any<CreatePupilPayment>()).Returns<PaymentResponseDto>(x => { throw dbException; });

            // Act
            var response = await _controller.PostAsync(_createPupilPayment.PupilId, _createPupilPayment);

            // Assert
            response.Should().BeOfType<ActionResult<PaymentResponseDto>>();
            response.Result.Should().BeOfType<BadRequestObjectResult>();
            BadRequestObjectResult result = (BadRequestObjectResult)response.Result;
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Value.Should().Be("An invalid operation occurred");
        }

        [Fact]
        public async Task PostAsync_ReturnsActionResultPaymentResponseDto()
        {
            // Arrange
            _mediator.Send(Arg.Any<CreatePupilPayment>()).Returns(_paymentDto);

            // Act
            var response = await _controller.PostAsync(_createPupilPayment.PupilId, _createPupilPayment);

            // Assert
            response.Should().BeOfType<ActionResult<PaymentResponseDto>>();
            response.Result.Should().BeOfType<CreatedAtRouteResult>();
            CreatedAtRouteResult result = (CreatedAtRouteResult)response.Result;
            result.StatusCode.Should().Be(StatusCodes.Status201Created);
            result.Value.Should().BeOfType<PaymentResponseDto>();
            var val = (PaymentResponseDto)result.Value;
            val.Type.Should().Be(_paymentDto.Type);
        }

    }
}