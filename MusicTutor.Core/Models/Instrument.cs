using System.Collections.Generic;

namespace MusicTutor.Core.Models
{
    public class Instrument 
    {
        public const int NameLength = 50;

        private Instrument() {}

        public Instrument(string name)
        {
            Name = name;
        }
        public int Id { get; private set; }
        
        public string Name { get; private set; }

        private HashSet<Pupil> _pupils;
        public IEnumerable<Pupil> Pupils => _pupils;
        
    }
}