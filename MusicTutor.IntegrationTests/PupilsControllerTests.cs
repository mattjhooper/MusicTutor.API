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
using MusicTutor.Api.Commands.Instruments;
using MusicTutor.Api.Contracts.Lessons;
using MusicTutor.Api.Contracts.Payments;
using MusicTutor.Core.Models.Enums;

namespace MusicTutor.IntegrationTests
{
    public class PupilsControllerTests : IntegrationTest
    {

        private readonly Faker _faker;

        public PupilsControllerTests(ApiWebApplicationFactory fixture, ITestOutputHelper testOutputHelper) : base(fixture, testOutputHelper)
        {
            _faker = new Faker("en_GB");
        }
        [Fact(Skip = SkipReason)]
        public async Task PupilCanBeCreatedUpdatedAndDeleted()
        {
            var pupilResponse = await CreateFakePupilAndValidate();

            await GetPupilAndValidate(pupilResponse);

            var updatePupilRequest = new UpdatePupil(pupilResponse.Id, "New Name", pupilResponse.LessonRate + 1, pupilResponse.StartDate.AddDays(-1), 7, "New Contact Name", "New Contact Email", "New PhoneNo");
            pupilResponse = await UpdatePupilAndValidate(updatePupilRequest);

            await DeletePupilAndValidate(pupilResponse.Id);
        }

        [Fact(Skip = SkipReason)]
        public async Task CanCreateLotsOfPupils()
        {
            for (int x = 0; x < 100; x++)
            {
                await CreateFakePupilAndValidate();
            }
        }

        [Fact(Skip = SkipReason)]
        public async Task DifferentUsersCannotAccessPupils()
        {

            var pupilResponse = await CreateFakePupilAndValidate();

            await RegisterNewUserAndSetBearerToken();

            await GetPupilAndValidate(pupilResponse, StatusCodes.Status404NotFound);

        }

        [Fact(Skip = SkipReason)]
        public async Task CanManipulatePupilInstruments()
        {

            var pupilResponse = await CreateFakePupilAndValidate();

            var pupilInstrumentsURI = $"{PupilsUri}/{pupilResponse.Id}/Instruments";

            List<InstrumentResponseDto> currentInstruments = await GetPupilInstrumentsAndValidate(pupilInstrumentsURI);

            // Find an instrument the pupil does not already have a link to
            InstrumentResponseDto newInstrument;
            do
            {
                newInstrument = await GetFakeInstrument();
            } while (currentInstruments.Any(i => i.Id == newInstrument.Id));

            await AddPupilInstrumentAndValidate(pupilResponse, pupilInstrumentsURI, newInstrument);

            await DeletePupilInstrumentAndValidate(pupilResponse, pupilInstrumentsURI, newInstrument);

        }

        [Fact(Skip = SkipReason)]
        public async Task CanManipulatePupilLessons()
        {

            var pupil = await CreateFakePupilAndValidate();
            pupil.AccountBalance.Should().Be(0);

            var pupilLessonsURI = $"{PupilsUri}/{pupil.Id}/Lessons";

            _output.WriteLine($"Add Lesson to Pupil: {pupil.Id}");
            var lesson = await AddPupilLessonAndValidate(pupil, pupilLessonsURI);

            var retrievedPupil = await GetPupil(pupil.Id);
            retrievedPupil.AccountBalance.Should().Be(-1 * lesson.Cost);

            List<LessonResponseDto> pupilLessons = await GetPupilLessonsAndValidate(pupil, pupilLessonsURI);

            pupilLessons.Count.Should().Be(1);

            await DeletePupilLessonAndValidate(pupil, pupilLessonsURI, pupilLessons);

            retrievedPupil = await GetPupil(pupil.Id);
            retrievedPupil.AccountBalance.Should().Be(0);

        }

        [Fact(Skip = SkipReason)]
        public async Task CanManipulatePupilPayments()
        {

            var pupil = await CreateFakePupilAndValidate();
            pupil.AccountBalance.Should().Be(0);

            var pupilPaymentsURI = $"{PupilsUri}/{pupil.Id}/Payments";

            _output.WriteLine($"Add Payment to Pupil: {pupil.Id}");
            var payment = await AddPupilPaymentAndValidate(pupil, pupilPaymentsURI);

            var retrievedPupil = await GetPupil(pupil.Id);
            retrievedPupil.AccountBalance.Should().Be(payment.Amount);

            List<PaymentResponseDto> pupilPayments = await GetPupilPaymentsAndValidate(pupil, pupilPaymentsURI);

            pupilPayments.Count.Should().Be(1);

            await DeletePupilPaymentAndValidate(pupil, pupilPaymentsURI, pupilPayments);

            retrievedPupil = await GetPupil(pupil.Id);
            retrievedPupil.AccountBalance.Should().Be(0);

        }

        private async Task<InstrumentResponseDto> GetInstrumentByName(string instrumentName)
        {
            _output.WriteLine($"Get {instrumentName}");
            var response = await _client.GetAsync(InstrumentsUri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsAsync<List<InstrumentResponseDto>>();
                var instrument = content.FirstOrDefault(i => i.Name == instrumentName);

                return instrument;
            }

            return null;
        }

        private async Task<InstrumentResponseDto> CreateInstrument(string instrumentName)
        {

            _output.WriteLine($"Post {instrumentName}");
            var createInstrument = new CreateInstrument(instrumentName);
            var response = await _client.PostAsJsonAsync<CreateInstrument>(InstrumentsUri, createInstrument);
            response.StatusCode.Should().Be(StatusCodes.Status201Created);

            var instrument = await response.Content.ReadAsAsync<InstrumentResponseDto>();

            instrument.Name.Should().Be(instrumentName);
            var id = instrument.Id;
            id.Should().NotBeEmpty();

            return instrument;
        }

        private async Task<InstrumentResponseDto> GetFakeInstrument()
        {
            var instrumentNames = new[] { "Drums", "Flute", "Guitar", "Piano", "Cello", "Saxophone" };

            var instrumentName = _faker.PickRandom(instrumentNames);

            var instrument = await GetInstrumentByName(instrumentName);

            if (instrument is null)
            {
                instrument = await CreateInstrument(instrumentName);
            }

            return instrument;
        }

        private async Task GetPupilAndValidate(PupilResponseDto pupil, int expectedResultCode = StatusCodes.Status200OK)
        {
            _output.WriteLine($"Get pupil");
            var response = await _client.GetAsync($"{PupilsUri}/{pupil.Id}");
            response.StatusCode.Should().Be(expectedResultCode);

            if (response.IsSuccessStatusCode)
            {
                var retrievedPupil = await response.Content.ReadAsAsync<PupilResponseDto>();
                retrievedPupil.Should().Be(pupil);
            }
        }

        private async Task<PupilResponseDto> GetPupil(Guid pupilId)
        {
            _output.WriteLine($"Retrieve Pupil: {pupilId}");

            var response = await _client.GetAsync($"{PupilsUri}/{pupilId}");
            response.StatusCode.Should().Be(StatusCodes.Status200OK);

            var retrievedPupil = await response.Content.ReadAsAsync<PupilResponseDto>();
            return retrievedPupil;
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

        private async Task<PupilResponseDto> CreateFakePupilAndValidate()
        {
            var instrument = await GetFakeInstrument();
            var contact = new Bogus.Person();
            var createPupilRequest = new CreatePupil(_faker.Name.FullName(), _faker.Finance.Amount(12, 20, 2), _faker.Date.Recent(7).Date, _faker.Random.ArrayElement<int>(new int[] { 7, 14 }), instrument.Id, contact.FullName, contact.Email, contact.Phone);
            var pupilResponse = await CreatePupilAndValidate(createPupilRequest);

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


        private async Task<List<InstrumentResponseDto>> GetPupilInstrumentsAndValidate(string pupilInstrumentsURI)
        {
            _output.WriteLine($"Get Instruments for Pupil: {pupilInstrumentsURI}");

            var response = await _client.GetAsync(pupilInstrumentsURI);
            response.StatusCode.Should().Be(StatusCodes.Status200OK);

            var currentInstruments = await response.Content.ReadAsAsync<List<InstrumentResponseDto>>();
            return currentInstruments;
        }

        private async Task DeletePupilInstrumentAndValidate(PupilResponseDto pupilResponse, string pupilInstrumentsURI, InstrumentResponseDto newInstrument)
        {
            _output.WriteLine($"Remove Instrument [{newInstrument.Name}] to Pupil: {pupilResponse.Id}");

            var deleteResponse = await _client.DeleteAsync($"{pupilInstrumentsURI}/{newInstrument.Id}");
            deleteResponse.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        private async Task AddPupilInstrumentAndValidate(PupilResponseDto pupilResponse, string pupilInstrumentsURI, InstrumentResponseDto newInstrument)
        {
            _output.WriteLine($"Add Instrument [{newInstrument.Name}] to Pupil: {pupilResponse.Id}");

            var postResponse = await _client.PostAsJsonAsync<CreatePupilInstrumentLink>(pupilInstrumentsURI, new CreatePupilInstrumentLink(pupilResponse.Id, newInstrument.Id));
            postResponse.StatusCode.Should().Be(StatusCodes.Status201Created);
        }

        private async Task<CreatePupilLesson> AddPupilLessonAndValidate(PupilResponseDto pupil, string pupilLessonsURI)
        {
            var fakeLesson = new CreatePupilLesson(pupil.Id, pupil.StartDate, 30, pupil.LessonRate, false);
            var postResponse = await _client.PostAsJsonAsync<CreatePupilLesson>(pupilLessonsURI, fakeLesson);
            postResponse.StatusCode.Should().Be(StatusCodes.Status201Created);
            return fakeLesson;
        }

        private async Task DeletePupilLessonAndValidate(PupilResponseDto pupil, string pupilLessonsURI, List<LessonResponseDto> pupilLessons)
        {
            _output.WriteLine($"Delete Pupil Lesson: {pupil.Id}");

            var deleteResponse = await _client.DeleteAsync($"{pupilLessonsURI}/{pupilLessons[0].Id}");
            deleteResponse.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        private async Task<List<LessonResponseDto>> GetPupilLessonsAndValidate(PupilResponseDto pupil, string pupilLessonsURI)
        {
            _output.WriteLine($"Retrieve Pupil Lessons: {pupil.Id}");

            var getLessonsResponse = await _client.GetAsync(pupilLessonsURI);

            getLessonsResponse.StatusCode.Should().Be(StatusCodes.Status200OK);

            var pupilLessons = await getLessonsResponse.Content.ReadAsAsync<List<LessonResponseDto>>();
            return pupilLessons;
        }

        private async Task<CreatePupilPayment> AddPupilPaymentAndValidate(PupilResponseDto pupil, string pupilPaymentsURI)
        {
            var fakePayment = new CreatePupilPayment(pupil.Id, pupil.StartDate, pupil.LessonRate, PaymentType.Cash);
            var postResponse = await _client.PostAsJsonAsync<CreatePupilPayment>(pupilPaymentsURI, fakePayment);
            postResponse.StatusCode.Should().Be(StatusCodes.Status201Created);
            return fakePayment;
        }

        private async Task DeletePupilPaymentAndValidate(PupilResponseDto pupil, string pupilPaymentsURI, List<PaymentResponseDto> pupilPayments)
        {
            _output.WriteLine($"Delete Pupil Payment: {pupil.Id}");

            var deleteResponse = await _client.DeleteAsync($"{pupilPaymentsURI}/{pupilPayments[0].Id}");
            deleteResponse.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        private async Task<List<PaymentResponseDto>> GetPupilPaymentsAndValidate(PupilResponseDto pupil, string pupilPaymentsURI)
        {
            _output.WriteLine($"Retrieve Pupil Payments: {pupil.Id}");

            var getPaymentsResponse = await _client.GetAsync(pupilPaymentsURI);

            getPaymentsResponse.StatusCode.Should().Be(StatusCodes.Status200OK);

            var pupilPayments = await getPaymentsResponse.Content.ReadAsAsync<List<PaymentResponseDto>>();
            return pupilPayments;
        }


    }
}
