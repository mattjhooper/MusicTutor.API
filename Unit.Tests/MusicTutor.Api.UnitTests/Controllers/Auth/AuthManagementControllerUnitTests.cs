using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MusicTutor.Api.Controllers.Auth;
using MusicTutor.Api.Settings;
using MusicTutor.Api.Commands.Auth;
using NSubstitute;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MusicTutor.Api.Contracts.Auth;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MusicTutor.Api.UnitTests.Controllers.Auth
{
    public class AuthManagementControllerUnitTests
    {

        private readonly AuthManagementController _sut;
        private readonly IMediator _mediator;

        public AuthManagementControllerUnitTests()
        {
            _mediator = Substitute.For<IMediator>();

            _sut = new AuthManagementController(_mediator);
        }

        [Fact]
        public void Register_ReturnsBadRequestForInvalidModel()
        {
            // Arrange
            var user = new RegisterUser("UserName", string.Empty, string.Empty);
            _sut.ModelState.TryAddModelError("TestError", "Invalid State");

            // Act
            var response = _sut.Register(user);

            // Assert
            response.Result.Should().BeOfType<BadRequestObjectResult>();
            BadRequestObjectResult result = (BadRequestObjectResult)response.Result;
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public void Register_ReturnsBadRequestForFalseResult()
        {
            // Arrange
            var user = new RegisterUser("UserName", "duplicate@email.com", string.Empty);
            var registrationResponse = new RegistrationResponseDto()
            {
                Result = false,
                Token = null,
                Errors = new List<string>(){
                                        "Error1"
                                    }
            };

            _mediator.Send(Arg.Any<RegisterUser>()).Returns(registrationResponse);

            // Act
            var response = _sut.Register(user);

            // Assert
            response.Result.Should().BeOfType<BadRequestObjectResult>();
            BadRequestObjectResult result = (BadRequestObjectResult)response.Result;
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public void Register_ReturnsTokenForTrueResult()
        {
            // Arrange
            var user = new RegisterUser("UserName", "new@email.com", string.Empty);
            var registrationResponse = new RegistrationResponseDto()
            {
                Result = true,
                Token = "exampleToken"
            };

            _mediator.Send(Arg.Any<RegisterUser>()).Returns(registrationResponse);

            // Act
            var response = _sut.Register(user);

            // Assert
            response.Result.Should().BeOfType<OkObjectResult>();
            OkObjectResult result = (OkObjectResult)response.Result;
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public void Login_ReturnsBadRequestForInvalidModel()
        {
            // Arrange
            var user = new LoginUser(string.Empty, string.Empty);
            _sut.ModelState.TryAddModelError("TestError", "Invalid State");

            // Act
            var response = _sut.Login(user);

            // Assert
            response.Result.Should().BeOfType<BadRequestObjectResult>();
            BadRequestObjectResult result = (BadRequestObjectResult)response.Result;
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public void Login_ReturnsBadRequestForFalseResult()
        {
            // Arrange
            var user = new LoginUser("duplicate@email.com", string.Empty);
            var registrationResponse = new RegistrationResponseDto()
            {
                Result = false,
                Token = null,
                Errors = new List<string>(){
                                        "Error1"
                                    }
            };

            _mediator.Send(Arg.Any<LoginUser>()).Returns(registrationResponse);

            // Act
            var response = _sut.Login(user);

            // Assert
            response.Result.Should().BeOfType<BadRequestObjectResult>();
            BadRequestObjectResult result = (BadRequestObjectResult)response.Result;
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public void Login_ReturnsTokenForTrueResult()
        {
            // Arrange
            var user = new LoginUser("new@email.com", string.Empty);
            var registrationResponse = new RegistrationResponseDto()
            {
                Result = true,
                Token = "exampleToken"
            };

            _mediator.Send(Arg.Any<LoginUser>()).Returns(registrationResponse);

            // Act
            var response = _sut.Login(user);

            // Assert
            response.Result.Should().BeOfType<OkObjectResult>();
            OkObjectResult result = (OkObjectResult)response.Result;
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }
    }
}