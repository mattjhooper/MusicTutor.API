using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using MusicTutor.Api.Contracts.Instruments;
using System.Linq;
using MusicTutor.Api.Commands.Pupils;
using MusicTutor.Api.Contracts.Pupils;
using Xunit.Abstractions;
using Bogus;

namespace MusicTutor.IntegrationTests
{
    public class PupilsControllerTests : IntegrationTest
    {

        public PupilsControllerTests(ApiWebApplicationFactory fixture, ITestOutputHelper testOutputHelper) : base(fixture, testOutputHelper) { }
        [Fact]
        public async Task PupilCanBeCreatedUpdatedAndDeleted()
        {
            var piano = await GetInstrument("Piano");

            var createPupilRequest = new CreatePupil("Pupil Name", 21M, DateTime.Today, 14, piano.Id, "Contact Name", "Contact Email", "Contact Phone Number");
            var pupilResponse = await CreatePupilAndValidate(createPupilRequest);

            await GetPupilAndValidate(pupilResponse);

            var updatePupilRequest = new UpdatePupil(pupilResponse.Id, "New Name", pupilResponse.LessonRate + 1, pupilResponse.StartDate.AddDays(-1), 7, "New Contact Name", "New Contact Email", "New PhoneNo");
            pupilResponse = await UpdatePupilAndValidate(updatePupilRequest);

            await DeletePupilAndValidate(pupilResponse.Id);
        }

        [Fact(Skip = "Don't create too much data")]
        public async Task CanCreateLotsOfPupils()
        {
            var instrumentNames = new[] { "Drums", "Flute", "Guitar", "Piano" };

            var faker = new Faker("en_GB");

            for (int x = 0; x < 100; x++)
            {
                var instrument = await GetInstrument(faker.PickRandom(instrumentNames));
                var contact = new Bogus.Person();
                var createPupilRequest = new CreatePupil(faker.Name.FullName(), faker.Finance.Amount(12, 20, 2), faker.Date.Recent(7).Date, faker.Random.ArrayElement<int>(new int[] { 7, 14 }), instrument.Id, contact.FullName, contact.Email, contact.Phone);
                var pupilResponse = await CreatePupilAndValidate(createPupilRequest);

                await GetPupilAndValidate(pupilResponse);
            }
        }

        private async Task<InstrumentResponseDto> GetInstrument(string instrumentName)
        {
            _output.WriteLine($"Get {instrumentName}");
            var response = await _client.GetAsync(InstrumentsUri);
            response.StatusCode.Should().Be(StatusCodes.Status200OK);

            var content = await response.Content.ReadAsAsync<List<InstrumentResponseDto>>();
            var instrument = content.Single(i => i.Name == instrumentName);

            return instrument;
        }

        private async Task GetPupilAndValidate(PupilResponseDto pupil)
        {
            _output.WriteLine($"Get pupil");
            var response = await _client.GetAsync($"{PupilsUri}/{pupil.Id}");
            response.StatusCode.Should().Be(StatusCodes.Status200OK);

            var retrievedPupil = await response.Content.ReadAsAsync<PupilResponseDto>();

            retrievedPupil.Should().Be(pupil);
        }

        private async Task<PupilResponseDto> CreatePupilAndValidate(CreatePupil createPupil)
        {
            _output.WriteLine($"Create pupil");
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
            _output.WriteLine($"Update pupil");
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
            _output.WriteLine($"Delete pupil");
            var response = await _client.DeleteAsync($"{PupilsUri}/{pupilId}");
            response.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }
    }
}
