using System.Collections.Generic;

namespace MusicTutorAPI.Core.Models
{
    public class Instrument 
    {
        private Instrument() {}

        public Instrument(string name)
        {
            Name = name;
        }
        public int Id { get; private set; }
        
        public string Name { get; private set; }
        
    }
}