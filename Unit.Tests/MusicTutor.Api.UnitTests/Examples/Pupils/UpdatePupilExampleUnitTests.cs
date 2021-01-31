using FluentAssertions;
using MusicTutor.Api.Commands.Pupils;
using MusicTutor.Api.Examples.Pupils;
using Xunit;

namespace MusicTutor.Api.UnitTests.Examples.Pupils
{
    public class UpdatePupilExampleUnitTests
    {
        [Fact]
        public void UpdatePupilExample_IsUpdatePupil()
        {
            //Given
            var updatePupilExample = new UpdatePupilExample();
        
            //When
            
            //Then
            updatePupilExample.GetExamples().Should().BeOfType<UpdatePupil>().Should().NotBeNull();
        }                
    }
}