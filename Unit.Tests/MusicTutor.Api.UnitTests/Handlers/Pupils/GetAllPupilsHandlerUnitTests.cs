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
using MusicTutor.Api.Contracts.Pupils;

namespace MusicTutor.Api.UnitTests.Handlers.Pupils
{
    public class GetAllPupilsHandlerUnitTests : PupilHandlerUnitTest
    {
        private readonly GetAllPupilsHandler _handler;
        public GetAllPupilsHandlerUnitTests()
        {
            _handler = new GetAllPupilsHandler(_dbContext, _mapper);
        }

        [Fact]
        public async Task GetPupilByIdHandler_ReturnsAllPupilsAsync()
        {
            //Given
            var getPupils = new GetAllPupils();
            
            //When
            var response = await _handler.Handle(getPupils, new CancellationToken());
            
            //Then    
            response.Count().Should().Be(1);
            var pupils = new List<PupilResponseDto>(response);
            pupils[0].Name.Should().Be(_pupil.Name);      
        }
    }
}