using MusicTutor.Api.EFCore.Handlers.Pupils;
using Xunit;
using System.Threading.Tasks;
using System.Threading;
using FluentAssertions;
using MusicTutor.Core.Models;
using System;
using MusicTutor.Api.Queries.Pupils;
using MusicTutor.Core.Models.Enums;

namespace MusicTutor.Api.UnitTests.Handlers.Pupils
{
    public class GetPupilPaymentByIdHandlerUnitTests : PupilHandlerUnitTest
    {
        private readonly GetPupilPaymentByIdHandler _handler;
        private readonly GetPupilPaymentById _getPupilPaymentById;

        public GetPupilPaymentByIdHandlerUnitTests()
        {
            _handler = new GetPupilPaymentByIdHandler(_dbContext, _mapper);
            var payment = Payment.CreatePayment(DateTime.Now, _pupil.CurrentLessonRate, PaymentType.Cash);
            _pupil.AddPayment(payment);
            _getPupilPaymentById = new GetPupilPaymentById(_pupil.Id, payment.Id);
        }

        [Fact]
        public async Task GetPupilPaymentByIdHandler_GetsPayment()
        {
            //Given
            //When
            var response = await _handler.Handle(_getPupilPaymentById, new CancellationToken());

            //Then    
            response.Id.Should().Be(_getPupilPaymentById.PaymentId);
        }

        [Fact]
        public async Task GetPupilPaymentByIdHandler_PupilNotFound_ReturnsNull()
        {
            //Given
            var unknownPupil = _getPupilPaymentById with { PupilId = Guid.NewGuid() };

            //When
            var response = await _handler.Handle(unknownPupil, new CancellationToken());

            //Then    
            response.Should().BeNull();
        }

        [Fact]
        public async Task GetPupilPaymentByIdHandler_PaymentNotFound_ReturnsNull()
        {
            //Given
            var unknownPayment = _getPupilPaymentById with { PaymentId = Guid.NewGuid() };

            //When
            var response = await _handler.Handle(unknownPayment, new CancellationToken());

            //Then    
            response.Should().BeNull();
        }
    }
}