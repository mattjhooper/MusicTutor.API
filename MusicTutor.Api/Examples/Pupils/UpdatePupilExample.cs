using System;
using MusicTutor.Api.Commands.Pupils;
using Swashbuckle.AspNetCore.Filters;

namespace MusicTutor.Api.Examples.Pupils
{
    public class UpdatePupilExample : IExamplesProvider<UpdatePupil>
    {
        public UpdatePupil GetExamples()
        {
            return new UpdatePupil(Guid.NewGuid(), "John", 15.0M, DateTime.Now, 7, "John's Mum", "john.mum@mailinator.com", "07890 123456");
        }
    }
}