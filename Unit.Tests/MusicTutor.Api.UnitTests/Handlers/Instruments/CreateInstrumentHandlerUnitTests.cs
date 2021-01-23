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

namespace MusicTutor.Api.UnitTests.Handlers.Instruments
{
    public class CreateInstrumentHandlerUnitTests
    {
        private readonly CreateInstrumentHandler _handler;
        private readonly IMusicTutorDbContext _dbContext;

        public CreateInstrumentHandlerUnitTests()
        {
            _dbContext = MockDbContextBuilder.Init().WithInstruments().Build();
            IMapper mapper = MappingBuilder.Init().Build();
            _handler = new CreateInstrumentHandler(_dbContext, mapper);
        }

        [Fact]
        public async Task CreateInstrumentHandler_AddsInstrumentAsync()
        {
            //Given
            var createInstrument = new CreateInstrument("Bongos");
            
            //When
            var response = await _handler.Handle(createInstrument, new CancellationToken());
            
            //Then    
            response.Name.Should().Be("Bongos");
            _dbContext.Instruments.Received().AddAsync(Arg.Any<Instrument>());
            _dbContext.Received().SaveChangesAsync(Arg.Any<CancellationToken>());
        }
    }
}