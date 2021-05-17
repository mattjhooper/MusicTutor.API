using MusicTutor.Api.EFCore.Handlers.Pupils;
using Xunit;
using MusicTutor.Api.Commands.Pupils;
using System.Threading.Tasks;
using System.Threading;
using NSubstitute;
using MusicTutor.Core.Models;
using System;

namespace MusicTutor.Api.UnitTests.Handlers.Pupils
{
    public class DeletePupilHandlerUnitTests : PupilHandlerUnitTest
    {
        private readonly DeletePupilHandler _handler;

        public DeletePupilHandlerUnitTests()
        {
            _handler = new DeletePupilHandler(_dbContext);
        }

        [Fact]
        public async Task DeletePupilHandler_DeletesPupilAsync()
        {
            //Given
            var deletePupil = new DeletePupil(_pupil.Id);

            //When
            var response = await _handler.Handle(deletePupil, new CancellationToken());

            //Then    
            _dbContext.Pupils.Received().Remove(Arg.Is<Pupil>(_pupil));
            await _dbContext.Received().SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task DeletePupilHandler_NotFound_NoDelete()
        {
            //Given
            var guid = Guid.NewGuid();
            var deletePupil = new DeletePupil(guid);

            //When
            var response = await _handler.Handle(deletePupil, new CancellationToken());

            //Then    
            // response.Should().Be(1);
            _dbContext.Pupils.DidNotReceive().Remove(Arg.Any<Pupil>());
            await _dbContext.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        }
    }
}