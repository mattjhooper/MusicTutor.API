using System.Collections.Generic;

namespace MusicTutor.Core.Contracts.Errors
{
    public class ErrorResponse
    {
        public List<ErrorModel> Errors { get; } = new List<ErrorModel>();
    }
} 