using FluentAssertions;
using MusicTutor.Api.Commands.Pupils;
using MusicTutor.Api.Examples.Pupils;
using Xunit;

namespace MusicTutor.Api.UnitTests.Examples.Pupils
{
    public class CreatePupilExampleUnitTests
    {
        [Fact]
        public void CreatePupilExample_IsCreatePupil()
        {
            //Given
            var createPupilExample = new CreatePupilExample();
        
            //When
            
            //Then
            createPupilExample.GetExamples().Should().BeOfType<CreatePupil>().Should().NotBeNull();
        }                
    }
}