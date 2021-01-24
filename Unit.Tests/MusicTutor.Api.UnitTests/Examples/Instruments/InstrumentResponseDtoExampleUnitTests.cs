using System.Collections.Generic;
using FluentAssertions;
using MusicTutor.Api.Contracts.Instruments;
using MusicTutor.Api.Examples.Instruments;
using Xunit;

namespace MusicTutor.Api.UnitTests.Examples.Instruments
{
    public class InstrumentResponseDtoExampleUnitTests
    {
        [Fact]
        public void InstrumentResponseDtoExample_IsInstrumentResponseDto()
        {
            //Given
            var instrumentResponseDtoExample = new InstrumentResponseDtoExample();
        
            //When
            
            //Then
            instrumentResponseDtoExample.GetExamples().Should().BeOfType<InstrumentResponseDto>().Should().NotBeNull();
        } 

        [Fact]
        public void InstrumentResponseDtoListExample_IsInstrumentResponseDtoList()
        {
            //Given
            var instrumentResponseDtoListExample = new InstrumentResponseDtoListExample();
        
            //When
            
            //Then
            instrumentResponseDtoListExample.GetExamples().Should().BeOfType<InstrumentResponseDto[]>().Should().NotBeNull();
        }            
    }
}