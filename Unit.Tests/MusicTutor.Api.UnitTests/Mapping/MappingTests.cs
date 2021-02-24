using Xunit;
using MapsterMapper;
using MusicTutor.Core.Models;
using MusicTutor.Api.Contracts.Instruments;
using FluentAssertions;
using System;
using MusicTutor.Api.Contracts.Pupils;
using MusicTutor.Api.Contracts.Lessons;

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

        [Fact]
        public void ShouldSupportLessonMapping()
        {
            //Given
            var lesson = Lesson.CreateLesson(DateTime.Now, 15, 15.5M);
            
            //When
            var lessonDto = _mapper.Map<LessonResponseDto>(lesson);

            //Then
            lessonDto.Should().BeOfType(typeof(LessonResponseDto));
            lessonDto.StartDateTime.Should().Be(lesson.StartDateTime);
            lessonDto.InstrumentId.Should().BeNull();
            lessonDto.InstrumentName.Should().BeNullOrEmpty();
            lessonDto.DurationInMinutes.Should().Be(lesson.DurationInMinutes);
            lessonDto.Cost.Should().Be(lesson.Cost);
            lessonDto.Status.Should().Be(Lesson.STATUS_COMPLETE);
            lessonDto.Id.Should().Be(lesson.Id);
        }

        [Fact]
        public void ShouldSupportLessonMappingWithInstrument()
        {
            //Given
            var instrument = Instrument.CreateInstrument("Piano");
            var lesson = Lesson.CreateLessonWithInstrument(DateTime.Now, 15, 15.5M, instrument, true);
            
            //When
            var lessonDto = _mapper.Map<LessonResponseDto>(lesson);

            //Then
            lessonDto.Should().BeOfType(typeof(LessonResponseDto));
            lessonDto.StartDateTime.Should().Be(lesson.StartDateTime);
            lessonDto.InstrumentId.Should().Be(instrument.Id);
            lessonDto.InstrumentName.Should().Be(instrument.Name);
            lessonDto.DurationInMinutes.Should().Be(lesson.DurationInMinutes);
            lessonDto.Cost.Should().Be(lesson.Cost);
            lessonDto.Status.Should().Be(Lesson.STATUS_PLANNED);
            lessonDto.Id.Should().Be(lesson.Id);
        }
    }
}