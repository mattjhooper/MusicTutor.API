using MapsterMapper;
using MusicTutor.Api.UnitTests.Mapping;
using MusicTutor.Api.EFCore.Handlers.Pupils;
using MusicTutor.Core.Services;
using MusicTutor.Api.UnitTests.Utils;
using Xunit;
using MusicTutor.Api.Commands.Pupils;
using System.Threading.Tasks;
using System.Threading;
using FluentAssertions;
using NSubstitute;
using MusicTutor.Core.Models;
using System.Linq;
using System;

namespace MusicTutor.Api.UnitTests.Handlers.Pupils
{
    public class CreatePupilHandlerUnitTests
    {
        private readonly CreatePupilHandler _handler;
        private readonly IMusicTutorDbContext _dbContext;

        private readonly Instrument _instrument;

        public CreatePupilHandlerUnitTests()
        {
            _instrument = Instrument.CreateInstrument("TEST");
            _dbContext = MockDbContextBuilder.Init().WithInstruments(_instrument).WithPupils().Build();
            IMapper mapper = MappingBuilder.Init().Build();
            _handler = new CreatePupilHandler(_dbContext, mapper);
        }

        [Fact]
        public async Task CreatePupilHandler_AddsPupilAsync()
        {
            //Given
            var createPupil = new CreatePupil("PupilName", 14M, DateTime.Now, 7, _instrument.Id, "ContactName", "ContactEmail", "ContactPhoneNumber" );
            
            //When
            var response = await _handler.Handle(createPupil, new CancellationToken());
            
            //Then    
            response.Name.Should().Be("PupilName");
            await _dbContext.Pupils.Received().AddAsync(Arg.Is<Pupil>(x => x.Name == "PupilName" && x.Contact.Name == "ContactName" && x.Instruments.Contains(_instrument)));
            await _dbContext.Received().SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task CreatePupilHandler_ThrowsExceptionForUnknownInstrumentAsync()
        {
            //Given
            var createPupil = new CreatePupil("PupilName", 14M, DateTime.Now, 7, Guid.NewGuid(), "ContactName", "ContactEmail", "ContactPhoneNumber" );
            
            //When
            Func<Task> act = async () => { await _handler.Handle(createPupil, new CancellationToken()); };
            await act.Should().ThrowAsync<InvalidOperationException>();
            
            //Then    
            await _dbContext.Pupils.DidNotReceive().AddAsync(Arg.Any<Pupil>());
            await _dbContext.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        }
    }
}