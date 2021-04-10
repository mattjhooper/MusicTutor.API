using System.Net.Http;
using System.Threading.Tasks;
using Bogus;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using MusicTutor.Api.Commands.Auth;
using MusicTutor.Api.Contracts.Auth;
using MusicTutor.Api.Controllers;
using Respawn;
using Xunit;
using Xunit.Abstractions;

// https://timdeschryver.dev/blog/how-to-test-your-csharp-web-api

namespace MusicTutor.IntegrationTests
{
    public abstract class IntegrationTest : IClassFixture<ApiWebApplicationFactory>
    {
        protected const string InstrumentsUri = "/" + BaseApiController.Instruments;
        protected const string PupilsUri = "/" + BaseApiController.Pupils;
        protected const string AuthUri = "/" + BaseApiController.Auth;
        protected readonly ITestOutputHelper _output;

        private readonly Checkpoint _checkpoint = new Checkpoint
        {
            SchemasToInclude = new[] {
            "dbo"
        },
            WithReseed = true
        };

        protected readonly ApiWebApplicationFactory _factory;
        protected readonly HttpClient _client;
        protected readonly IConfiguration _configuration;

        public IntegrationTest(ApiWebApplicationFactory fixture, ITestOutputHelper testOutputHelper)
        {
            _output = testOutputHelper;
            _factory = fixture;
            _client = _factory.CreateClient();
            _configuration = new ConfigurationBuilder()
                  .AddJsonFile("appsettings.Integration.json")
                  .Build();

            var token = GetTokenAsync().Result;
            _output.WriteLine($"Token: {token}");
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);



            //     _output.WriteLine("Checkpoint Reset");
            //     _checkpoint.Reset(_configuration.GetConnectionString("Default")).Wait();
        }

        protected async Task<string> GetTokenAsync()
        {
            var user = new Bogus.Person();

            var registerUser = new RegisterUser(user.UserName, user.Email, "CompliantPassword123!");
            _output.WriteLine($"RegisterUser request: {registerUser}");

            var response = await _client.PostAsJsonAsync<RegisterUser>($"{AuthUri}/Register", registerUser);

            var registrationResponse = await response.Content.ReadAsAsync<RegistrationResponseDto>();

            return registrationResponse.Token;
        }
    }
}