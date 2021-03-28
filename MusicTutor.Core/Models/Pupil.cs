using System;
using System.Collections.Generic;
using System.Linq;
using Ardalis.GuardClauses;
using MusicTutor.Core.Base;

namespace MusicTutor.Core.Models
{
    public class Pupil : ModelBase
    {

        public const int NameLength = 150;

        public Guid Id { get; private set; }

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
        { }

        public void AddCompletedLesson(DateTime startDateTime, int durationInMinutes, decimal cost)
        {
            _lessons.Add(new Lesson(startDateTime, durationInMinutes, cost, false));
            AccountBalance -= cost;
        }

        public void AddPlannedLesson(DateTime startDateTime, int durationInMinutes, decimal cost)
        {
            _lessons.Add(new Lesson(startDateTime, durationInMinutes, cost, true));
        }

        public void AddLesson(Lesson lesson)
        {
            _lessons.Add(lesson);
            if (!lesson.IsPlanned)
            {
                AccountBalance -= lesson.Cost;
            }
        }

        public bool RemoveLesson(Lesson lesson)
        {
            CheckLessonsCollectionIsLoaded();

            if (!lesson.IsPlanned)
            {
                AccountBalance += lesson.Cost;
            }

            return _lessons.Remove(lesson);
        }



        public string InstrumentsToString()
        {
            CheckInstrumentCollectionIsLoaded();

            return _instruments.Select(i => i.Name).Aggregate("", (str, next) => str + next + " ").Trim();
        }

        public bool AddInstrument(Instrument instrument)
        {
            CheckInstrumentCollectionIsLoaded();

            return _instruments.Add(instrument);
        }

        public bool HasInstrument(Guid instrumentId)
        {
            CheckInstrumentCollectionIsLoaded();

            return _instruments.Any(i => i.Id == instrumentId);
        }

        public bool CanRemoveInstrument(Guid instrumentId)
        {
            CheckInstrumentCollectionIsLoaded();

            return HasInstrument(instrumentId) && _instruments.Count() > 1;
        }

        public int RemoveInstrument(Guid instrumentId)
        {
            CheckInstrumentCollectionIsLoaded();

            if (!CanRemoveInstrument(instrumentId))
                return 0;

            return _instruments.RemoveWhere(i => i.Id == instrumentId);
        }

        public void AddPayment(Payment payment)
        {
            _payments.Add(payment);
            AccountBalance += payment.Amount;
        }

        private void CheckInstrumentCollectionIsLoaded()
        {
            if (_instruments == null)
                throw new InvalidOperationException("The Instruments collection must be loaded before calling this method");
        }

        private void CheckLessonsCollectionIsLoaded()
        {
            if (_lessons == null)
                throw new InvalidOperationException("The Lessons collection must be loaded before calling this method");
        }

        public void UpdatePupil(string name, decimal currentLessonRate, DateTime startDate, int frequencyInDays, string contactName, string contactEmail, string contactPhone)
        {
            Guard.Against.NullOrWhiteSpace(name, nameof(name));
            Guard.Against.NullOrWhiteSpace(contactName, nameof(contactName));
            Name = name;
            CurrentLessonRate = currentLessonRate;
            StartDate = startDate;
            FrequencyInDays = frequencyInDays;

            Contact newContact = new(contactName, contactEmail, contactPhone);
            if (newContact != Contact)
                Contact = newContact;


        }

        public static Pupil CreatePupil(string name, decimal currentLessonRate, DateTime startDate, int frequencyInDays, ICollection<Instrument> instruments, string contactName, string contactEmail = null, string contactPhone = null)
        {
            return new Pupil(name, currentLessonRate, startDate, frequencyInDays, instruments, new Contact(contactName, contactEmail, contactPhone));
        }

    }
}