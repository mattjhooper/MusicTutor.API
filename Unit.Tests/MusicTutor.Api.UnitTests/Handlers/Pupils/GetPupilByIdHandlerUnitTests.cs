using MapsterMapper;
using MusicTutor.Api.UnitTests.Mapping;
using MusicTutor.Api.EFCore.Handlers.Pupils;
using MusicTutor.Core.Services;
using MusicTutor.Api.UnitTests.Utils;
using Xunit;
using MusicTutor.Api.Queries.Pupils;
using System.Threading.Tasks;
using System.Threading;
using FluentAssertions;
using NSubstitute;
using MusicTutor.Core.Models;
using System.Linq;
using System;
using System.Collections.Generic;

namespace MusicTutor.Api.UnitTests.Handlers.Pupils
{
    public class GetPupilByIdHandlerUnitTests : PupilHandlerUnitTest
    {
        private readonly GetPupilByIdHandler _handler;
        
        public GetPupilByIdHandlerUnitTests()
        {
            _handler = new GetPupilByIdHandler(_dbContext, _mapper);
        }

        [Fact]
        public async Task GetPupilByIdHandler_ReturnsPupilAsync()
        {
            //Given
            var getPupil = new GetPupilById(_pupil.Id);
            
            //When
            var response = await _handler.Handle(getPupil, new CancellationToken());
            
            //Then    
            response.Name.Should().Be(_pupil.Name);
        }
    }
}