using System.Collections.Generic;

namespace MusicTutor.Api.Contracts.Auth
{
    public class RegistrationResponseDto
    {
        public string Token { get; set; }
        public bool Result { get; set; }
        public List<string> Errors { get; set; }
    }

}