using System.Collections.Generic;

namespace MusicTutorAPI.Core.Models
{
    public record Contact(string name, string email = null, string phoneNumber = null);     
}