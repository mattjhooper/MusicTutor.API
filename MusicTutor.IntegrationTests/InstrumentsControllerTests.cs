using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using MusicTutor.Api.Commands.Instruments;
using MusicTutor.Api.Contracts.Instruments;
using Xunit.Abstractions;

namespace MusicTutor.IntegrationTests
{
    public class InstrumentsControllerTests : IntegrationTest
    {

        public InstrumentsControllerTests(ApiWebApplicationFactory fixture, ITestOutputHelper testOutputHelper) : base(fixture, testOutputHelper) { }

        [Fact(Skip = "Don't run in CI")]
        public async Task GetAllInstruments()
        {
            _output.WriteLine("Get all Instruments");
            var response = await _client.GetAsync(InstrumentsUri);
            response.StatusCode.Should().Be(StatusCodes.Status200OK);

            var content = await response.Content.ReadAsAsync<List<InstrumentResponseDto>>();

            content.Count.Should().Be(4);
        }

        [Fact(Skip = "Don't run in CI")]
        public async Task ShouldBeAbleToAddInstrumentAndRemoveIt()
        {
            _output.WriteLine("Create Triangle");
            var createInstrument = new CreateInstrument("Triangle");
            var response = await _client.PostAsJsonAsync<CreateInstrument>(InstrumentsUri, createInstrument);
            response.StatusCode.Should().Be(StatusCodes.Status201Created);

            var content = await response.Content.ReadAsAsync<InstrumentResponseDto>();

            content.Name.Should().Be("Triangle");
            var id = content.Id;
            id.Should().NotBeEmpty();

            _output.WriteLine("Get Triangle");
            response = await _client.GetAsync($"{InstrumentsUri}/{id}");
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            content = await response.Content.ReadAsAsync<InstrumentResponseDto>();
            content.Name.Should().Be("Triangle");

            response = await _client.DeleteAsync($"{InstrumentsUri}/{id}");
            response.StatusCode.Should().Be(StatusCodes.Status204NoContent);

        }
    }
}
