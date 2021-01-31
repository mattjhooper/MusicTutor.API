using System;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using MusicTutor.Api;
using MusicTutor.Core.Models;
using Xunit;
using Microsoft.AspNetCore.TestHost;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using MusicTutor.Api.Commands.Instruments;
using MusicTutor.Api.Contracts.Instruments;

namespace MusicTutor.IntegrationTests
{
    public class InstrumentControllerTests: IntegrationBase
    {        
        [Fact]
        public async Task GetAllInstruments()
        {

            var response = await _client.GetAsync("/Instruments");
            response.StatusCode.Should().Be(StatusCodes.Status200OK);

            var content = await response.Content.ReadAsAsync<List<InstrumentResponseDto>>();

            content.Count.Should().Be(4);
        }

        [Fact]
        public async Task ShouldBeAbleToAddInstrumentAndRemoveIt()
        {

            var createInstrument = new CreateInstrument("Triangle");
            var response = await _client.PostAsJsonAsync<CreateInstrument>("/Instruments", createInstrument);
            response.StatusCode.Should().Be(StatusCodes.Status201Created);

            var content = await response.Content.ReadAsAsync<InstrumentResponseDto>();

            content.Name.Should().Be("Triangle");
            var id = content.Id;
            id.Should().NotBeEmpty();

            response = await _client.GetAsync($"/Instruments/{id}");
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            content = await response.Content.ReadAsAsync<InstrumentResponseDto>();
            content.Name.Should().Be("Triangle");

            response = await _client.DeleteAsync($"/Instruments/{id}");
            response.StatusCode.Should().Be(StatusCodes.Status204NoContent);

        }



    }
}
