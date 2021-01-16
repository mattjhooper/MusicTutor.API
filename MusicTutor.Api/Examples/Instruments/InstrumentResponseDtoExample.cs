using System;
using System.Collections.Generic;
using MusicTutor.Api.Contracts.Instruments;
using Swashbuckle.AspNetCore.Filters;

namespace MusicTutor.Api.Examples.Instruments
{
    public class InstrumentResponseDtoExample : IExamplesProvider<InstrumentResponseDto>
    {
        public InstrumentResponseDto GetExamples()
        {
            return new InstrumentResponseDto(Guid.NewGuid(), "Triangle");            
        }
    }

    public class InstrumentResponseDtoListExample : IExamplesProvider<IEnumerable<InstrumentResponseDto>>
    {
        public IEnumerable<InstrumentResponseDto> GetExamples()
        {
           return new InstrumentResponseDto[] { new InstrumentResponseDto(Guid.NewGuid(), "Triangle"), new InstrumentResponseDto(Guid.NewGuid(), "Bagpipes") } ;
        }
    }
}