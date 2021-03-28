using System;
using MusicTutor.Core.Models.Enums;

namespace MusicTutor.Core.Models
{
    public class Payment
    {
        public Guid Id { get; private set; }
        public DateTime PaymentDate { get; private set; }

        public decimal Amount { get; private set; }

        public Guid PupilId { get; private set; }

        public Pupil Pupil { get; private set; }

        public PaymentType Type { get; private set; }

        private Payment() { }

        public Payment(DateTime paymentDate, decimal amount, PaymentType paymentType)
        {
            PaymentDate = paymentDate;
            Amount = amount;
            Type = paymentType;
        }

    }
}