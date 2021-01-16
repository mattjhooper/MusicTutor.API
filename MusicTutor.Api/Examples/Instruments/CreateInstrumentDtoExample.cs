using MusicTutor.Api.Commands.Instruments;
using Swashbuckle.AspNetCore.Filters;

namespace MusicTutor.Api.Examples.Instruments
{
    public class CreateInstrumentDtoExample : IExamplesProvider<CreateInstrument>
    {
        public CreateInstrument GetExamples()
        {
            return new CreateInstrument("Triangle");
        }
    }
}