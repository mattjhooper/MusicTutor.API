using System;
using System.Collections.Generic;
using Ardalis.GuardClauses;

namespace MusicTutor.Core.Models
{
    public class Instrument : IUserAssignable
    {
        public const int NameLength = 50;

        private Instrument() { }

        public Instrument(string name)
        {
            Guard.Against.NullOrWhiteSpace(name, nameof(name));
            Name = name;
        }
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        private HashSet<Pupil> _pupils;
        public IEnumerable<Pupil> Pupils => _pupils;

        public Guid MusicTutorUserId { get; private set; }

        public MusicTutorUser MusicTutorUser { get; private set; }

        public void AssignToMusicTutorUser(Guid musicTutorUserId)
        {
            MusicTutorUserId = musicTutorUserId;
        }

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