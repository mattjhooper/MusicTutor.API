using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using MusicTutorAPI.Core.Base;

namespace MusicTutorAPI.Core.Models
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

        public Pupil(string name, decimal currentLessonRate, DateTime startDate, int frequencyInDays, Contact contact) : this()
        {
            Name = name;
            CurrentLessonRate = currentLessonRate;
            StartDate = startDate;
            FrequencyInDays = frequencyInDays;  
            Contact = contact;          
        }
        
        public Pupil(string name, decimal currentLessonRate, DateTime startDate, int frequencyInDays, string contactName, string contactEmail = null, string contactPhone = null) : 
            this(name, currentLessonRate, startDate, frequencyInDays, new Contact(contactName, contactEmail, contactPhone))
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

    }
}