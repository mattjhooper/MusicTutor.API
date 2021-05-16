using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using MusicTutor.Api.Commands.Instruments;
using MusicTutor.Api.Contracts.Instruments;
using Xunit.Abstractions;
using System.Linq;
using System;

namespace MusicTutor.IntegrationTests
{
    public class InstrumentsControllerTests : IntegrationTest
    {

        public InstrumentsControllerTests(ApiWebApplicationFactory fixture, ITestOutputHelper testOutputHelper) : base(fixture, testOutputHelper) { }

        [Fact]
        public async Task ShouldBeAbleToAddInstrumentAndRemoveIt()
        {

            const string INSTRUMENT_NAME = "Triangle";

            var instrument = await CreateInstrument(INSTRUMENT_NAME);

            instrument = await GetInstrumentById(instrument.Id);
            instrument.Name.Should().Be(INSTRUMENT_NAME);

            instrument = await GetInstrumentByName(INSTRUMENT_NAME);
            instrument.Name.Should().Be(INSTRUMENT_NAME);

            var response = await _client.DeleteAsync($"{InstrumentsUri}/{instrument.Id}");
            response.StatusCode.Should().Be(StatusCodes.Status204NoContent);

        }

        [Fact]
        public async Task DifferentUsersCannotAccessInstruments()
        {

            const string INSTRUMENT_NAME = "Banjo";

            var instrument = await CreateInstrument(INSTRUMENT_NAME);

            instrument = await GetInstrumentById(instrument.Id);
            instrument.Name.Should().Be(INSTRUMENT_NAME);

            await RegisterNewUserAndSetBearerToken();

            var instrument2 = await GetInstrumentById(instrument.Id, StatusCodes.Status404NotFound);

            instrument2.Should().BeNull();

        }

        private async Task<InstrumentResponseDto> CreateInstrument(string instrumentName)
        {
            _output.WriteLine($"Create {instrumentName}");
            var createInstrument = new CreateInstrument(instrumentName);
            var response = await _client.PostAsJsonAsync<CreateInstrument>(InstrumentsUri, createInstrument);
            response.StatusCode.Should().Be(StatusCodes.Status201Created);

            var content = await response.Content.ReadAsAsync<InstrumentResponseDto>();

            content.Name.Should().Be(instrumentName);
            var id = content.Id;
            id.Should().NotBeEmpty();

            return content;
        }

        private async Task<InstrumentResponseDto> GetInstrumentByName(string instrumentName)
        {
            _output.WriteLine($"Get {instrumentName}");
            var response = await _client.GetAsync(InstrumentsUri);
            response.StatusCode.Should().Be(StatusCodes.Status200OK);

            var content = await response.Content.ReadAsAsync<List<InstrumentResponseDto>>();
            var instrument = content.Single(i => i.Name == instrumentName);

            return instrument;
        }

        private async Task<InstrumentResponseDto> GetInstrumentById(Guid instrumentId, int expectedResultCode = StatusCodes.Status200OK)
        {
            _output.WriteLine($"Get {instrumentId}");

            var response = await _client.GetAsync($"{InstrumentsUri}/{instrumentId}");
            response.StatusCode.Should().Be(expectedResultCode);

            if (response.IsSuccessStatusCode)
            {
                var instrument = await response.Content.ReadAsAsync<InstrumentResponseDto>();
                return instrument;
            }

            return null;
        }


    }
}
