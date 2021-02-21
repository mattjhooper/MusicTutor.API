using System;
using MusicTutor.Core.Base;

namespace MusicTutor.Core.Models
{
    public class Lesson : ModelBase 
    {
        private Lesson() {}

        internal Lesson(DateTime startDateTime, int durationInMinutes, decimal cost, bool isPlanned = false) 
        {
            StartDateTime = startDateTime;
            DurationInMinutes = durationInMinutes;
            Cost = cost;
            IsPlanned = isPlanned;
        }


        public Guid Id { get; private set; }
        public DateTime StartDateTime { get; private set; }

        public int DurationInMinutes { get; private set; }

        public decimal Cost { get; private set; } 

        public Guid PupilId { get; private set; }

        public Pupil Pupil { get; private set; }        

        public Guid? InstrumentId { get; set; }
        public Instrument Instrument { get; set; }
    
        public bool IsPlanned { get; private set; } = false;

        public string Status => IsPlanned? "Planned" : "Complete";

    }
}