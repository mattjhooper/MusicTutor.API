using Xunit;
using MapsterMapper;
using MusicTutor.Core.Models;
using MusicTutor.Api.Contracts.Instruments;
using FluentAssertions;
using System;
using MusicTutor.Api.Contracts.Pupils;

namespace MusicTutor.Api.UnitTests.Mapping
{
    public class MappingTests
    {
        private readonly IMapper _mapper;
        public MappingTests()
        {   
            _mapper = MappingBuilder.Init().Build();
        }

        [Fact]
        public void ShouldSupportInstrumentMapping()
        {
            //Given
            var instrument = new Instrument("Flute");
            
            //When
            var instrumentDto = _mapper.Map<InstrumentResponseDto>(instrument);

            //Then
            instrumentDto.Should().BeOfType(typeof(InstrumentResponseDto));
            instrumentDto.Name.Should().Be(instrument.Name);
        }

        [Fact]
        public void ShouldSupportPupilMapping()
        {
            //Given
            var instruments = new Instrument[] { new Instrument("Piano") };
            var pupil = Pupil.CreatePupil("Name", 15.0M, DateTime.Now, 7, instruments, "ContactName", "ContactEmail", "ContactPhone");
            //When
            var pupilDto = _mapper.Map<PupilResponseDto>(pupil);            
            
            //Then
            pupilDto.LessonRate.Should().Be(pupil.CurrentLessonRate);
            
        }
    }
}