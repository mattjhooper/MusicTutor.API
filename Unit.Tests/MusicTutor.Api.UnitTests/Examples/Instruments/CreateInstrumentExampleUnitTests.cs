using FluentAssertions;
using MusicTutor.Api.Commands.Instruments;
using MusicTutor.Api.Examples.Instruments;
using Xunit;

namespace MusicTutor.Api.UnitTests.Examples.Instruments
{
    public class CreateInstrumentExampleUnitTests
    {
        [Fact]
        public void CreateInstrumentExample_IsCreateInstrument()
        {
            //Given
            var createInstrumentExample = new CreateInstrumentExample();
        
            //When
            
            //Then
            createInstrumentExample.GetExamples().Should().BeOfType<CreateInstrument>().Should().NotBeNull();
        }                
    }
}