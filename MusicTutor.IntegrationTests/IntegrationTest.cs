using System.Net.Http;
using Microsoft.Extensions.Configuration;
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

       //     _output.WriteLine("Checkpoint Reset");
       //     _checkpoint.Reset(_configuration.GetConnectionString("Default")).Wait();
        }
    }
}