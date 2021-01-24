using FluentAssertions;
using MusicTutor.Api.Contracts.Errors;
using MusicTutor.Api.Examples.Errors;
using Xunit;

namespace MusicTutor.Api.UnitTests.Examples.Errors
{
    public class ErrorResponseExampleUnitTests
    {
        [Fact]
        public void ErrorResponseExample_IsCorrect()
        {
            //Given
            var errorResponseExample = new ErrorResponseExample();
        
            //When
            
            //Then
            errorResponseExample.GetExamples().Should().BeOfType<ErrorResponse>().Should().NotBeNull();
        }                
    }
}