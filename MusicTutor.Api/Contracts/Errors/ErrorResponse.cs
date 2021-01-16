using System.Collections.Generic;

namespace MusicTutor.Api.Contracts.Errors
{
    public class ErrorResponse
    {
        public List<ErrorModel> Errors { get; } = new List<ErrorModel>();
    }
} 