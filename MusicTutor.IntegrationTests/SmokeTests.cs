using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;
using Xunit.Abstractions;

namespace MusicTutor.IntegrationTests
{
    public class SmokeTests : IntegrationTest
    {
        public SmokeTests(ApiWebApplicationFactory fixture, ITestOutputHelper testOutputHelper)
      : base(fixture, testOutputHelper) { }

        [Theory]
        [InlineData("/Instruments")]
        [InlineData("/Pupils")]
        public async Task Smoketest_Should_ResultInOK(string endpoint)
        {
            _output.WriteLine($"Get endpoint: {endpoint}");
            var response = await _client.GetAsync(endpoint);
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
        }
    }
}