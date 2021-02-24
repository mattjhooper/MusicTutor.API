using System;
using MusicTutor.Core.Base;

namespace MusicTutor.Core.Models
{
    public class Lesson : ModelBase 
    {
        public const string STATUS_PLANNED = "Planned";
        public const string STATUS_COMPLETE = "Complete";

        private Lesson() {}

        public Lesson(DateTime startDateTime, int durationInMinutes, decimal cost, bool isPlanned = false) 
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

        public string Status => IsPlanned? STATUS_PLANNED : STATUS_COMPLETE;

        public static Lesson CreateLesson(DateTime startDateTime, int durationInMinutes, decimal cost, bool isPlanned = false)
        {
            var lesson = new Lesson
            {
                Id = Guid.NewGuid(),
                StartDateTime = startDateTime,
                DurationInMinutes = durationInMinutes,
                Cost = cost,
                IsPlanned = isPlanned
            };

            return lesson;
        }

        public static Lesson CreateLessonWithInstrument(DateTime startDateTime, int durationInMinutes, decimal cost, Instrument instrument, bool isPlanned = false)
        {
            var lesson = CreateLesson(startDateTime, durationInMinutes, cost, isPlanned);
            lesson.InstrumentId = instrument.Id;
            lesson.Instrument = instrument;

            return lesson;
        }

    }
}