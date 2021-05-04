using MusicTutor.Api.EFCore.Handlers.Pupils;
using Xunit;
using System.Threading.Tasks;
using System.Threading;
using FluentAssertions;
using System.Linq;
using System;
using MusicTutor.Api.Commands.Pupils;
using NSubstitute;
using MusicTutor.Core.Models.Enums;
using MusicTutor.Api.Contracts.Payments;
using MusicTutor.Api.Commands.Auth;

namespace MusicTutor.Api.UnitTests.Handlers.Pupils
{
    public class CreatePupilPaymentHandlerUnitTests : PupilHandlerUnitTest
    {
        private readonly CreatePupilPaymentHandler _handler;
        private readonly CreatePupilPayment _createPupilPayment;

        public CreatePupilPaymentHandlerUnitTests()
        {
            _handler = new CreatePupilPaymentHandler(_dbContext, _mapper);
            _createPupilPayment = new CreatePupilPayment(_pupil.Id, _pupil.StartDate, _pupil.CurrentLessonRate, PaymentType.Cash);
        }

        [Fact]
        public async Task CreatePupilPaymentHandler_CreatesPaymentsAsync()
        {
            //Given
            var existingBalance = _pupil.AccountBalance;
            var req = new WithMusicTutorUserId<CreatePupilPayment, PaymentResponseDto>(_currentUser.Id, _createPupilPayment);
            //When
            var response = await _handler.Handle(req, new CancellationToken());

            //Then    
            response.Amount.Should().Be(_createPupilPayment.Amount);
            response.Type.Should().Be(_createPupilPayment.Type);
            _pupil.Payments.Count().Should().Be(1);
            _pupil.AccountBalance.Should().Be(existingBalance + _createPupilPayment.Amount);
            _pupil.Payments.Any(p => p.Id == response.Id);
            await _dbContext.Received().SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task CreatePupilPaymentHandler_NotFound_ReturnsNull()
        {
            //Given
            var unknownPupil = _createPupilPayment with { PupilId = Guid.NewGuid() };
            var req = new WithMusicTutorUserId<CreatePupilPayment, PaymentResponseDto>(_currentUser.Id, unknownPupil);

            //When
            var response = await _handler.Handle(req, new CancellationToken());

            //Then    
            response.Should().BeNull();
        }
    }
}