using MapsterMapper;
using MusicTutor.Api.UnitTests.Mapping;
using MusicTutor.Api.EFCore.Handlers.Pupils;
using MusicTutor.Core.Services;
using MusicTutor.Api.UnitTests.Utils;
using Xunit;
using System.Threading.Tasks;
using System.Threading;
using FluentAssertions;
using MusicTutor.Core.Models;
using System.Linq;
using System;
using System.Collections.Generic;
using MusicTutor.Api.Queries.Pupils;
using MusicTutor.Core.Models.Enums;
using MusicTutor.Api.Commands.Pupils;
using MusicTutor.Api.Contracts.Payments;

namespace MusicTutor.Api.UnitTests.Handlers.Pupils
{
    public class GetPupilPaymentsHandlerUnitTests : PupilHandlerUnitTest
    {
        private readonly GetPupilPaymentsHandler _handler;

        public GetPupilPaymentsHandlerUnitTests()
        {
            var payment = Payment.CreatePayment(DateTime.Now, _pupil.CurrentLessonRate, PaymentType.Cash);
            _pupil.AddPayment(payment);
            _handler = new GetPupilPaymentsHandler(_dbContext, _mapper);
        }

        [Fact]
        public async Task GetPupilPaymentsHandler_GetsPaymentsAsync()
        {
            //Given
            var getPupilPayments = new GetPupilPayments(_pupil.Id);
            var req = new WithMusicTutorUserId<GetPupilPayments, IEnumerable<PaymentResponseDto>>(_currentUser.Id, getPupilPayments);

            //When
            var response = await _handler.Handle(req, new CancellationToken());

            //Then    
            response.Count().Should().Be(1);
        }

        [Fact]
        public async Task GetPupilPaymentsHandler_NotFound_ReturnsNull()
        {
            //Given
            var guid = Guid.NewGuid();
            var getPupilPayments = new GetPupilPayments(guid);
            var req = new WithMusicTutorUserId<GetPupilPayments, IEnumerable<PaymentResponseDto>>(_currentUser.Id, getPupilPayments);

            //When
            var response = await _handler.Handle(req, new CancellationToken());

            //Then    
            response.Should().BeNull();
        }
    }
}