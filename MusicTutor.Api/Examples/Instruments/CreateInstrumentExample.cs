using MusicTutor.Api.Commands.Instruments;
using Swashbuckle.AspNetCore.Filters;

namespace MusicTutor.Api.Examples.Instruments
{
    public class CreateInstrumentExample : IExamplesProvider<CreateInstrument>
    {
        public CreateInstrument GetExamples()
        {
            return new CreateInstrument("Triangle");
        }
    }
}