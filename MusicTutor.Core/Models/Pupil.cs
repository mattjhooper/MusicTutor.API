using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MusicTutor.Core.Base;

namespace MusicTutor.Core.Models
{
    public class Pupil : ModelBase
    {

        public const int NameLength = 150;
        
        public int Id { get; private set; }

        public string Name { get; private set; }

        public Contact Contact { get; private set; }
        public decimal CurrentLessonRate { get; private set; }
        public decimal AccountBalance { get; private set; } = 0.0m;

        public DateTime StartDate { get; private set; }

        public Boolean IsActive { get; private set; } = true;

        public int FrequencyInDays { get; private set; }

        public byte[] Timestamp { get; private set; }

        private List<Payment> _payments;
        public IEnumerable<Payment> Payments => _payments;

        private List<Lesson> _lessons;
        public IEnumerable<Lesson> Lessons => _lessons;

        private HashSet<Instrument> _instruments;
        public IEnumerable<Instrument> Instruments => _instruments;


        private Pupil()
        {
            _payments = new List<Payment>();
            _lessons = new List<Lesson>();
        }

        public Pupil(string name, decimal currentLessonRate, DateTime startDate, int frequencyInDays, ICollection<Instrument> instruments, Contact contact) : this()
        {
            Name = name;
            CurrentLessonRate = currentLessonRate;
            StartDate = startDate;
            FrequencyInDays = frequencyInDays;  
            Contact = contact;
            
            if (instruments is null)
                throw new ArgumentNullException(nameof(instruments));

            _instruments = new HashSet<Instrument>(instruments);

            if (!instruments.Any())            
                throw new InvalidOperationException("Pupil must have at least one instrument");
        }
        
        public Pupil(string name, decimal currentLessonRate, DateTime startDate, int frequencyInDays, ICollection<Instrument> instruments, string contactName, string contactEmail = null, string contactPhone = null) : 
            this(name, currentLessonRate, startDate, frequencyInDays, instruments, new Contact(contactName, contactEmail, contactPhone))
        {}

        public void AddCompletedLesson(DateTime startDateTime, int durationInMinutes, decimal cost)
        {
            _lessons.Add(new Lesson(startDateTime, durationInMinutes, cost, false));
            AccountBalance -= cost;
        }

        public void AddPlannedLesson(DateTime startDateTime, int durationInMinutes, decimal cost)
        {
            _lessons.Add(new Lesson(startDateTime, durationInMinutes, cost, true));
        }

        public string InstrumentsToString()
        {
            if (_instruments == null)
                throw new InvalidOperationException("The Instruments collection must be loaded before calling this method");
            
            return _instruments.Select(i => i.Name).Aggregate("", (str, next) => str + next + " ").Trim();
        } 

        public static Pupil CreatePupil(string name, decimal currentLessonRate, DateTime startDate, int frequencyInDays, ICollection<Instrument> instruments, string contactName, string contactEmail = null, string contactPhone = null)
        {
            return new Pupil(name, currentLessonRate, startDate, frequencyInDays, instruments, new Contact(contactName, contactEmail, contactPhone));
        }

    }
}