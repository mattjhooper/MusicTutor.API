using MapsterMapper;
using MusicTutor.Api.UnitTests.Mapping;
using MusicTutor.Api.EFCore.Handlers.Pupils;
using MusicTutor.Core.Services;
using MusicTutor.Api.UnitTests.Utils;
using Xunit;
using MusicTutor.Api.Commands.Pupils;
using System.Threading.Tasks;
using System.Threading;
using FluentAssertions;
using NSubstitute;
using MusicTutor.Core.Models;
using System.Linq;
using System;
using System.Collections.Generic;
using MusicTutor.Api.Contracts.Pupils;

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
            //Given
            var req = new WithMusicTutorUserId<UpdatePupil, PupilResponseDto>(_currentUser.Id, _updatePupil);

            //When
            var response = await _handler.Handle(req, new CancellationToken());

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
            var req = new WithMusicTutorUserId<UpdatePupil, PupilResponseDto>(_currentUser.Id, unknownPupil);

            //When
            var response = await _handler.Handle(req, new CancellationToken());
            response.Should().BeNull();

            //Then    
            await _dbContext.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        }
    }
}