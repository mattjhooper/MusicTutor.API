using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MusicTutor.Api.Commands.Auth;
using MusicTutor.Api.EFCore.Handlers.Auth;
using MusicTutor.Services.Auth;
using NSubstitute;
using Xunit;

namespace MusicTutor.Api.UnitTests.Handlers.Auth
{
    public class LoginUserHandlerUnitTests
    {
        private readonly LoginUserHandler _handler;
        private readonly IAuthService _authService;

        private readonly LoginUser _user;

        private readonly string _token;

        public LoginUserHandlerUnitTests()
        {
            _user = new LoginUser("user@email.com", "password");
            _token = "MyToken";
            _authService = Substitute.For<IAuthService>();
            _authService.FindUserByEmailAsync(Arg.Any<string>()).Returns<MusicTutorUser>(new MusicTutorUser());
            _authService.CheckPasswordAsync(Arg.Any<MusicTutorUser>(), Arg.Any<string>()).Returns<bool>(true);
            _authService.GenerateJwtToken(Arg.Any<MusicTutorUser>()).Returns<string>(_token);

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
        public async Task Handle_UnknownUser_ReturnsError()
        {
            //Given
            _authService.FindUserByEmailAsync(Arg.Any<string>()).Returns<MusicTutorUser>((MusicTutorUser)null);

            //When
            var response = await _handler.Handle(_user, new CancellationToken());

            //Then
            response.Result.Should().BeFalse();
            response.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Handle_CheckPasswordFail_ReturnsError()
        {
            //Given
            _authService.CheckPasswordAsync(Arg.Any<MusicTutorUser>(), Arg.Any<string>()).Returns<bool>(false);

            //When
            var response = await _handler.Handle(_user, new CancellationToken());

            //Then
            response.Result.Should().BeFalse();
            response.Errors.Count.Should().BeGreaterThan(0);
        }
    }
}