using System.Collections.Generic;
using MusicTutor.Api.Controllers.Instruments.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace MusicTutor.Api.Controllers.Instruments.Dtos
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