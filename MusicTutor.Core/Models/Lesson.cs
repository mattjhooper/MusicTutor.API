using System;

namespace MusicTutorAPI.Core.Models
{
    public class Lesson 
    {
        public int Id { get; set; }
        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public decimal Cost { get; set; } 

        public int PupilId { get; set; }

        public Pupil Pupil { get; set; }

        // public int InstrumentId { get; set; }
        // public Instrument Instrument { get; set; }
    
        public Boolean IsPlanned { get; set; } = true;

    }
}