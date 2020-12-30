using System.Collections.Generic;

namespace MusicTutorAPI.Core.Models
{
    public record Contact(string Name, string Email = null, string PhoneNumber = null)
    {
        public const int NameLength = 150;
        public const int EmailLength = 150;
        public const int PhoneNumberLength = 30;
    }
}