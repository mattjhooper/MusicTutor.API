using MusicTutor.Core.Contracts.Instruments;
using Swashbuckle.AspNetCore.Filters;

namespace MusicTutor.Api.Examples.Instruments
{
    public class CreateInstrumentDtoExample : IExamplesProvider<CreateInstrumentDto>
    {
        public CreateInstrumentDto GetExamples()
        {
            return new CreateInstrumentDto("Triangle", "Percussion");
        }
    }
}