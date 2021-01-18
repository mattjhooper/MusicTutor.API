using Xunit;
using System.Linq;
using FluentAssertions;


namespace MusicTutor.Api.UnitTests.Utils
{
    public class MockDbContextBuilderTests
    {
        [Fact]
        public void ShouldReturnInstruments()
        {
            //Given
            var dbContext = MockDbContextBuilder.Init().WithInstruments().Build();
            
            //When
            var instruments = dbContext.Instruments.ToList();

            //Then
            instruments.Count().Should().Be(2);

        }
    }
}