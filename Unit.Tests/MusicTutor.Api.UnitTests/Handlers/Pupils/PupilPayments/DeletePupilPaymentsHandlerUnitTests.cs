using MusicTutor.Api.EFCore.Handlers.Pupils;
using Xunit;
using System.Threading.Tasks;
using System.Threading;
using FluentAssertions;
using MusicTutor.Core.Models;
using System.Linq;
using System;
using MusicTutor.Api.Commands.Pupils;
using NSubstitute;
using MusicTutor.Core.Models.Enums;

namespace MusicTutor.Api.UnitTests.Handlers.Pupils
{
    public class DeletePupilPaymentHandlerUnitTests : PupilHandlerUnitTest
    {
        private readonly DeletePupilPaymentHandler _handler;
        private readonly DeletePupilPayment _deletePupilPayment;

        public DeletePupilPaymentHandlerUnitTests()
        {
            _handler = new DeletePupilPaymentHandler(_dbContext);
            var payment = Payment.CreatePayment(DateTime.Now, _pupil.CurrentLessonRate, PaymentType.Cash);
            _pupil.AddPayment(payment);
            _deletePupilPayment = new DeletePupilPayment(_pupil.Id, payment.Id);
        }

        [Fact]
        public async Task DeletePupilPaymentHandler_DeletesPaymentsAsync()
        {
            //Given
            var existingBalance = _pupil.AccountBalance;
            //When
            var response = await _handler.Handle(_deletePupilPayment, new CancellationToken());

            //Then    
            response.Should().Be(1);
            _pupil.Payments.Count().Should().Be(0);
            _pupil.AccountBalance.Should().Be(existingBalance - _pupil.CurrentLessonRate);
            await _dbContext.Received().SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task DeletePupilPaymentHandler_PupilNotFound_ReturnsMinus1()
        {
            //Given
            var unknownPupil = _deletePupilPayment with { PupilId = Guid.NewGuid() };

            //When
            var response = await _handler.Handle(unknownPupil, new CancellationToken());

            //Then    
            response.Should().Be(-1);
        }

        [Fact]
        public async Task DeletePupilPaymentHandler_PaymentNotFound_Returns0()
        {
            //Given
            var unknownPayment = _deletePupilPayment with { PaymentId = Guid.NewGuid() };

            //When
            var response = await _handler.Handle(unknownPayment, new CancellationToken());

            //Then    
            response.Should().Be(0);
        }
    }
}