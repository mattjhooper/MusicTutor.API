using System.Collections.Generic;
using MusicTutor.Core.Contracts.Instruments;
using Swashbuckle.AspNetCore.Filters;

namespace MusicTutor.Api.Examples.Instruments
{
    public class InstrumentResponseDtoExample : IExamplesProvider<InstrumentResponseDto>
    {
        public InstrumentResponseDto GetExamples()
        {
            return new InstrumentResponseDto(101, "Triangle");            
        }
    }

    public class InstrumentResponseDtoListExample : IExamplesProvider<IEnumerable<InstrumentResponseDto>>
    {
        public IEnumerable<InstrumentResponseDto> GetExamples()
        {
           return new InstrumentResponseDto[] { new InstrumentResponseDto(101, "Triangle"), new InstrumentResponseDto(102, "Bagpipes") } ;
        }
    }
}