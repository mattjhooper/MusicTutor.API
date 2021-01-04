using MusicTutor.Core.Contracts.Errors;
using Swashbuckle.AspNetCore.Filters;

namespace MusicTutor.Api.Examples.Instruments
{
    public class ErrorResponseExample : IExamplesProvider<ErrorResponse>
    {
        public ErrorResponse GetExamples()
        {
            var errorResponse = new ErrorResponse();
            
            errorResponse.Errors.Add(new ErrorModel("Name", "'Name' must not be empty."));

            return errorResponse;            
        }
    }
}