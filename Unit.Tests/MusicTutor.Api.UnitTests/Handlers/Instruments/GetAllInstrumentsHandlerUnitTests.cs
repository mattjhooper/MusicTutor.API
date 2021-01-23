using MapsterMapper;
using MusicTutor.Api.UnitTests.Mapping;
using MusicTutor.Api.EFCore.Handlers.Instruments;
using MusicTutor.Core.Services;
using MusicTutor.Api.UnitTests.Utils;
using Xunit;
using MusicTutor.Api.Commands.Instruments;
using System.Threading.Tasks;
using System.Threading;
using FluentAssertions;
using NSubstitute;
using MusicTutor.Core.Models;
using System.Linq;
using MusicTutor.Api.Queries.Instruments;

namespace MusicTutor.Api.UnitTests.Handlers.Instruments
{
    public class GetAllInstrumentsHandlerUnitTests
    {
        private readonly GetAllInstrumentsHandler _handler;
        private readonly IMusicTutorDbContext _dbContext;

        public GetAllInstrumentsHandlerUnitTests()
        {
            _dbContext = MockDbContextBuilder.Init().WithInstruments().Build();
            _handler = new GetAllInstrumentsHandler(_dbContext);
        }

        [Fact]
        public async Task GetAllInstrumentsHandler_ReturnsInstrumentsAsync()
        {
            //Given
            var getAllInstruments = new GetAllInstruments();
            
            //When
            var response = await _handler.Handle(getAllInstruments, new CancellationToken());
            
            //Then    
            response.Count().Should().Be(_dbContext.Instruments.Count());
        }
    }
}