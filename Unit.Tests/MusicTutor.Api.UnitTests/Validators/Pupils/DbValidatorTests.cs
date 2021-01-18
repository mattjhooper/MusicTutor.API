using System;
using FluentValidation.TestHelper;
using MusicTutor.Api.Commands.Pupils;
using MusicTutor.Api.Validators.Pupils;
using NSubstitute;
using Xunit;
using MusicTutor.Api.UnitTests.Utils;
using MusicTutor.Core.Models;
using System.Threading.Tasks;
using System.Threading;
using FluentAssertions;

namespace MusicTutor.Api.UnitTests.Validators.Pupils
{
    public class DbValidatorTests
    {
        [Fact]
        public async Task InstrumentAlreadyExistsAsync_TrueAsync()
        {
            //Given
            var instrument = Instrument.CreateInstrument("Drums");
            var dbContext = MockDbContextBuilder.Init().WithInstruments(instrument).Build();
            var dbValidator = new DbValidator(dbContext);

            //When
            CancellationTokenSource source = new CancellationTokenSource();
            var result = await dbValidator.InstrumentAlreadyExistsAsync(instrument.Id, source.Token);
            
            //Then
            result.Should().BeTrue();
        }

        [Fact]
        public async Task InstrumentAlreadyExistsAsync_FalseAsync()
        {
            //Given
            var dbContext = MockDbContextBuilder.Init().WithInstruments().Build();
            var dbValidator = new DbValidator(dbContext);

            //When
            CancellationTokenSource source = new CancellationTokenSource();
            var result = await dbValidator.InstrumentAlreadyExistsAsync(Guid.NewGuid(), source.Token);
            
            //Then
            result.Should().BeFalse();
        }

   }
}