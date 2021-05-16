using MusicTutor.Api.EFCore.Handlers.Pupils;
using Xunit;
using MusicTutor.Api.Commands.Pupils;
using System.Threading.Tasks;
using System.Threading;
using FluentAssertions;
using NSubstitute;
using System;
using MusicTutor.Api.Contracts.Pupils;
using MusicTutor.Api.Commands.Auth;

namespace MusicTutor.Api.UnitTests.Handlers.Pupils
{
    public class UpdatePupilHandlerUnitTests : PupilHandlerUnitTest
    {
        private readonly UpdatePupilHandler _handler;
        private readonly UpdatePupil _updatePupil;

        public UpdatePupilHandlerUnitTests()
        {
            _handler = new UpdatePupilHandler(_dbContext, _mapper);
            _updatePupil = new UpdatePupil(_pupil.Id, "NewName", 15M, _pupil.StartDate.AddHours(1), 14, "NewContactName", "NewContactEmail", "NewContactPhoneNumber");
        }

        [Fact]
        public async Task UpdatePupilHandler_UpdatesPupilAsync()
        {
            //When
            var response = await _handler.Handle(_updatePupil, new CancellationToken());

            //Then    
            response.Name.Should().Be(_updatePupil.Name);
            response.LessonRate.Should().Be(_updatePupil.LessonRate);
            response.StartDate.Should().Be(_updatePupil.StartDate);
            response.FrequencyInDays.Should().Be(_updatePupil.FrequencyInDays);
            response.ContactName.Should().Be(_updatePupil.ContactName);
            response.ContactEmail.Should().Be(_updatePupil.ContactEmail);
            response.ContactPhoneNumber.Should().Be(_updatePupil.ContactPhoneNumber);
            await _dbContext.Received().SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task CreatePupilHandler_ReturnsNullForUnknownPupilAsync()
        {
            //Given
            var unknownPupil = _updatePupil with { Id = Guid.NewGuid() };

            //When
            var response = await _handler.Handle(unknownPupil, new CancellationToken());
            response.Should().BeNull();

            //Then    
            await _dbContext.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        }
    }
}