using System;
using System.Collections.Generic;
using Ardalis.GuardClauses;

namespace MusicTutor.Core.Models
{
    public class Instrument 
    {
        public const int NameLength = 50;

        private Instrument() {}

        public Instrument(string name)
        {
            Guard.Against.NullOrWhiteSpace(name, nameof(name));
            Name = name;

            _pupils = new HashSet<Pupil>();
        }
        public Guid Id { get; private set; }
        
        public string Name { get; private set; }

        private HashSet<Pupil> _pupils;
        public IEnumerable<Pupil> Pupils => _pupils;

        public static Instrument CreateInstrument(string name)
        {
            var instrument = new Instrument
            {
                Id = Guid.NewGuid(),
                Name = name
            };

            return instrument;
        }
        
    }
}