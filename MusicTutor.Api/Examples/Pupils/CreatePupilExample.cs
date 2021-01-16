using System;
using MusicTutor.Api.Commands.Pupils;
using Swashbuckle.AspNetCore.Filters;

namespace MusicTutor.Api.Examples.Pupils
{
    public class CreatePupilExample : IExamplesProvider<CreatePupil>
    {
        public CreatePupil GetExamples()
        {
            return new CreatePupil("John", 15.0M, DateTime.Now, 7, Guid.NewGuid(), "John's Mum", "john.mum@mailinator.com", "07890 123456");
        }
    }
}