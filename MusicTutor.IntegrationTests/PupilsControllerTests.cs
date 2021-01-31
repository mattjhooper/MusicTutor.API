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
using System.Linq;
using MusicTutor.Api.Commands.Pupils;
using MusicTutor.Api.Contracts.Pupils;

namespace MusicTutor.IntegrationTests
{
    public class PupilsControllerTests: IntegrationBase
    {        
        [Fact]
        public async Task PupilCanBeCreatedUpdatedAndDeleted()
        {
            var piano = await GetPiano();

            var createPupilRequest = new CreatePupil("Pupil Name", 21M, DateTime.Today, 14, piano.Id, "Contact Name", "Contact Email", "Contact Phone Number");
            var pupilResponse = await CreatePupilAndValidate(createPupilRequest);

            await GetPupilAndValidate(pupilResponse);

            var updatePupilRequest = new UpdatePupil(pupilResponse.Id, "New Name", pupilResponse.LessonRate + 1, pupilResponse.StartDate.AddDays(-1), 7, "New Contact Name", "New Contact Email", "New PhoneNo" );
            pupilResponse = await UpdatePupilAndValidate(updatePupilRequest);

            await DeletePupilAndValidate(pupilResponse.Id);
        }

        private async Task<InstrumentResponseDto> GetPiano()
        {
            var response = await _client.GetAsync(InstrumentsUri);
            response.StatusCode.Should().Be(StatusCodes.Status200OK);

            var content = await response.Content.ReadAsAsync<List<InstrumentResponseDto>>();
            var piano = content.Single(i => i.Name == "Piano");

            return piano;
        }

        private async Task GetPupilAndValidate(PupilResponseDto pupil)
        {
            var response = await _client.GetAsync($"{PupilsUri}/{pupil.Id}");
            response.StatusCode.Should().Be(StatusCodes.Status200OK);

            var retrievedPupil = await response.Content.ReadAsAsync<PupilResponseDto>();
            
            retrievedPupil.Should().Be(pupil);
        }

        private async Task<PupilResponseDto> CreatePupilAndValidate(CreatePupil createPupil)
        {
            var response = await _client.PostAsJsonAsync<CreatePupil>(PupilsUri, createPupil);
            response.StatusCode.Should().Be(StatusCodes.Status201Created);

            var pupilResponse = await response.Content.ReadAsAsync<PupilResponseDto>();
            pupilResponse.Id.Should().NotBeEmpty();
            pupilResponse.Name.Should().Be(createPupil.Name);
            pupilResponse.LessonRate.Should().Be(createPupil.LessonRate);
            pupilResponse.FrequencyInDays.Should().Be(createPupil.FrequencyInDays);
            pupilResponse.StartDate.Should().Be(createPupil.StartDate);
            pupilResponse.ContactName.Should().Be(createPupil.ContactName);            
            pupilResponse.ContactEmail.Should().Be(createPupil.ContactEmail);
            pupilResponse.ContactPhoneNumber.Should().Be(createPupil.ContactPhoneNumber);

            return pupilResponse;
        }

        private async Task<PupilResponseDto> UpdatePupilAndValidate(UpdatePupil updatePupil)
        {
            var response = await _client.PutAsJsonAsync<UpdatePupil>($"{PupilsUri}/{updatePupil.Id}", updatePupil);
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            var pupilResponse = await response.Content.ReadAsAsync<PupilResponseDto>();
            pupilResponse.Name.Should().Be(updatePupil.Name);
            pupilResponse.LessonRate.Should().Be(updatePupil.LessonRate);
            pupilResponse.FrequencyInDays.Should().Be(updatePupil.FrequencyInDays);
            pupilResponse.StartDate.Should().Be(updatePupil.StartDate);
            pupilResponse.ContactName.Should().Be(updatePupil.ContactName);            
            pupilResponse.ContactEmail.Should().Be(updatePupil.ContactEmail);
            pupilResponse.ContactPhoneNumber.Should().Be(updatePupil.ContactPhoneNumber);

            return pupilResponse;
        }

        private async Task DeletePupilAndValidate(Guid pupilId)
        {
            var response = await _client.DeleteAsync($"{PupilsUri}/{pupilId}");
            response.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }
    }
}
