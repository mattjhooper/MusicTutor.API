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
using System;

namespace MusicTutor.Api.UnitTests.Handlers.Instruments
{
    public class GetInstrumentByIdUnitTests
    {
        private readonly GetInstrumentByIdHandler _handler;
        private readonly IMusicTutorDbContext _dbContext;

        public GetInstrumentByIdUnitTests()
        {
            _dbContext = MockDbContextBuilder.Init().WithInstruments().Build();
            _handler = new GetInstrumentByIdHandler(_dbContext);
        }

        [Fact]
        public async Task GetInstrumentByIdHandler_ReturnsInstrumentAsync()
        {
            //Given
            var instrument = _dbContext.Instruments.ToList()[0];
            var getInstrumentById = new GetInstrumentById(instrument.Id);
            
            //When
            var response = await _handler.Handle(getInstrumentById, new CancellationToken());
            
            //Then    
            response.Name.Should().Be(instrument.Name);
        }

        [Fact]
        public async Task GetInstrumentByIdHandler_NotFoundReturnsNullAsync()
        {
            //Given
            var getInstrumentById = new GetInstrumentById(Guid.NewGuid());
            
            //When
            var response = await _handler.Handle(getInstrumentById, new CancellationToken());
            
            //Then    
            response.Should().BeNull();
        }
    }
}