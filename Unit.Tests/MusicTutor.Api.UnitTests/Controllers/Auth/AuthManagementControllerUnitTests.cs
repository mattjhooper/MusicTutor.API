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
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IOptionsMonitor<JwtConfig> _optionsMonitor;


        public AuthManagementControllerUnitTests()
        {
            _mediator = Substitute.For<IMediator>();

            _optionsMonitor = Substitute.For<IOptionsMonitor<JwtConfig>>();
            var store = Substitute.For<IUserStore<IdentityUser>>();
            var optionsAccessor = Substitute.For<IOptions<IdentityOptions>>();
            var passwordHasher = Substitute.For<IPasswordHasher<IdentityUser>>();
            var userValidators = Substitute.For<IEnumerable<IUserValidator<IdentityUser>>>();
            var passwordValidators = Substitute.For<IEnumerable<IPasswordValidator<IdentityUser>>>();
            var keyNormalizer = Substitute.For<ILookupNormalizer>();
            var errors = Substitute.For<IdentityErrorDescriber>();
            var services = Substitute.For<System.IServiceProvider>();
            var logger = Substitute.For<Microsoft.Extensions.Logging.ILogger<UserManager<IdentityUser>>>();
            _userManager = new UserManager<IdentityUser>(store,
                                                         optionsAccessor,
                                                         passwordHasher,
                                                         userValidators,
                                                         passwordValidators,
                                                         keyNormalizer,
                                                         errors,
                                                         services,
                                                         logger);

            _sut = new AuthManagementController(_userManager, _optionsMonitor, _mediator);
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

        [Fact(Skip = "Cannot mock FindByEmailAsync")]
        public void Register_ReturnsBadRequestForDuplicateEmail()
        {
            // Arrange
            var user = new RegisterUser("UserName", string.Empty, string.Empty);
            _userManager.FindByEmailAsync(Arg.Any<string>()).Returns<IdentityUser>((IdentityUser)null);


            // Act
            var response = _sut.Register(user);

            // Assert
            response.Result.Should().BeOfType<BadRequestObjectResult>();
            BadRequestObjectResult result = (BadRequestObjectResult)response.Result;
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }
    }
}