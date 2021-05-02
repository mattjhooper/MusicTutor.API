using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using MusicTutor.Api.Commands.Auth;
using MusicTutor.Api.EFCore.Handlers.Auth;
using MusicTutor.Services.Auth;
using NSubstitute;
using Xunit;

namespace MusicTutor.Api.UnitTests.Handlers.Auth
{
    public class RegisterUserHandlerUnitTests
    {
        private readonly RegisterUserHandler _handler;
        private readonly IAuthService _authService;

        private readonly RegisterUser _user;

        private readonly string _token;

        public RegisterUserHandlerUnitTests()
        {
            _user = new RegisterUser("Name", "user@email.com", "password");
            _token = "MyToken";
            _authService = Substitute.For<IAuthService>();
            _authService.FindIdentityUserByEmailAsync(Arg.Any<string>()).Returns<IdentityUser>((IdentityUser)null);

            _authService.CreateIdentityUserAsync(Arg.Any<IdentityUser>(), Arg.Any<string>()).Returns<IdentityResult>(IdentityResult.Success);

            _authService.GenerateJwtToken(Arg.Any<IdentityUser>()).Returns<string>(_token);

            _handler = new(_authService);

        }

        [Fact]
        public async Task Handle_ReturnsToken()
        {
            //When
            var response = await _handler.Handle(_user, new CancellationToken());

            //Then
            response.Result.Should().BeTrue();
            response.Token.Should().Be(_token);
        }

        [Fact]
        public async Task Handle_ExistingUser_ReturnsError()
        {
            //Given
            _authService.FindIdentityUserByEmailAsync(Arg.Any<string>()).Returns<IdentityUser>(new IdentityUser(_user.Email));


            //When
            var response = await _handler.Handle(_user, new CancellationToken());

            //Then
            response.Result.Should().BeFalse();
            response.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Handle_CreateIdentityUserAsyncFail_ReturnsError()
        {
            //Given
            _authService.CreateIdentityUserAsync(Arg.Any<IdentityUser>(), Arg.Any<string>()).Returns<IdentityResult>(IdentityResult.Failed(new IdentityError[] { new IdentityError() }));

            //When
            var response = await _handler.Handle(_user, new CancellationToken());

            //Then
            response.Result.Should().BeFalse();
            // response.Errors.Count.Should().BeGreaterThan(0);
        }
    }
}