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
    public class CreatePupilHandlerUnitTests : PupilHandlerUnitTest
    {
        private readonly CreatePupilHandler _handler;
        private readonly CreatePupil _newPupil;
        
        public CreatePupilHandlerUnitTests() : base()
        {
            _handler = new CreatePupilHandler(_dbContext, _mapper);
            _newPupil = new CreatePupil("NewPupilName", 14M, DateTime.Now, 7, _instrument.Id, "ContactName", "ContactEmail", "ContactPhoneNumber" );
        }

        [Fact]
        public async Task CreatePupilHandler_AddsPupilAsync()
        {
            //Given
            //When
            var response = await _handler.Handle(_newPupil, new CancellationToken());
            
            //Then    
            response.Name.Should().Be(_newPupil.Name);
            await _dbContext.Pupils.Received().AddAsync(Arg.Is<Pupil>(x => x.Name == _newPupil.Name && x.Contact.Name == _newPupil.ContactName && x.Instruments.Contains(_instrument)));
            await _dbContext.Received().SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task CreatePupilHandler_ThrowsExceptionForUnknownInstrumentAsync()
        {
            //Given
            var pupilUnknownInstrument = _newPupil with { DefaultInstrumentId = Guid.NewGuid() };
            
            //When
            Func<Task> act = async () => { await _handler.Handle(pupilUnknownInstrument, new CancellationToken()); };
            await act.Should().ThrowAsync<InvalidOperationException>();
            
            //Then    
            await _dbContext.Pupils.DidNotReceive().AddAsync(Arg.Any<Pupil>());
            await _dbContext.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        }
    }
}