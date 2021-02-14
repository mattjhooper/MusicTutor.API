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
using MusicTutor.Core.Services;

namespace MusicTutor.Api.UnitTests.Validators.Pupils
{
    public class DbValidatorTests
    {
        private readonly Instrument _instrument;
        private readonly Pupil _pupil;
        private readonly IMusicTutorDbContext _dbContext;

        private readonly DbValidator _sut;

        private readonly DeletePupilInstrumentLink _deletePupilInstrumentLink;

        public DbValidatorTests()
        {
            _instrument = Instrument.CreateInstrument("Drums");
            _pupil = Pupil.CreatePupil("PupilName", 14M, DateTime.Now, 7,new Instrument[] {_instrument}, "ContactName", "ContactEmail", "ContactPhoneNumber" );
            
            _dbContext = MockDbContextBuilder.Init().WithInstruments(_instrument).WithPupils(_pupil).Build();

            _sut = new DbValidator(_dbContext);

            _deletePupilInstrumentLink = new DeletePupilInstrumentLink(_pupil.Id, _instrument.Id);
        }
        
        [Fact]
        public async Task InstrumentAlreadyExistsAsync_TrueAsync()
        {
            //Given
            //When
            var result = await _sut.InstrumentAlreadyExistsAsync(_instrument.Id, new CancellationToken());
            
            //Then
            result.Should().BeTrue();
        }

        [Fact]
        public async Task InstrumentAlreadyExistsAsync_FalseAsync()
        {
            //Given
            //When
            var result = await _sut.InstrumentAlreadyExistsAsync(Guid.NewGuid(), new CancellationToken());
            
            //Then
            result.Should().BeFalse();
        }

        [Fact]
        public async Task PupilAlreadyExistsAsync_TrueAsync()
        {
            //Given
            //When
            var result = await _sut.PupilAlreadyExistsAsync(_pupil.Id, new CancellationToken());
            
            //Then
            result.Should().BeTrue();
        }

        [Fact]
        public async Task PupilAlreadyExistsAsync_FalseAsync()
        {
            //Given
            //When
            var result = await _sut.PupilAlreadyExistsAsync(Guid.NewGuid(), new CancellationToken());
            
            //Then
            result.Should().BeFalse();
        }

        [Fact]
        public async Task PupilInstrumentCanBeRemovedAsync_PupilDoesNotExists_FalseAsync()
        {
            //Given
            var unknownPupil = _deletePupilInstrumentLink with { pupilId = Guid.NewGuid() };

            //When
            var result = await _sut.PupilInstrumentCanBeRemovedAsync(unknownPupil, new CancellationToken());
            
            //Then
            result.Should().BeFalse();
        }

        [Fact]
        public async Task PupilInstrumentCanBeRemovedAsync_PupilOneInstrument_FalseAsync()
        {
            //Given
            //When
            var result = await _sut.PupilInstrumentCanBeRemovedAsync(_deletePupilInstrumentLink, new CancellationToken());
            
            //Then
            result.Should().BeFalse();
        }

        [Fact]
        public async Task PupilInstrumentCanBeRemovedAsync_PupilUnknownInstrument_FalseAsync()
        {
            //Given
            var unknownInstrument = _deletePupilInstrumentLink with { instrumentId = Guid.NewGuid() };

            //When
            CancellationTokenSource source = new CancellationTokenSource();
            var result = await _sut.PupilInstrumentCanBeRemovedAsync(unknownInstrument, source.Token);
            
            //Then
            result.Should().BeFalse();
        }

        [Fact]
        public async Task PupilInstrumentCanBeRemovedAsync_SecondInstrument_TrueAsync()
        {
            //Given
            var secondInstrument = Instrument.CreateInstrument("Cello");
            _pupil.AddInstrument(secondInstrument);

            var deleteSecondInstrument = _deletePupilInstrumentLink with { instrumentId = secondInstrument.Id };

            //When
            CancellationTokenSource source = new CancellationTokenSource();
            var result = await _sut.PupilInstrumentCanBeRemovedAsync(deleteSecondInstrument, source.Token);
            
            //Then
            result.Should().BeTrue();
        }

   }
}