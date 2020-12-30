using System;

namespace MusicTutorAPI.Core.Models
{
    public class Payment 
    {
        public enum PaymentType
        {
            Cash,
            BankTransfer,
            Cheque,
            Other
        }
        
        public int Id { get; set; }
        public DateTime PaymentDate { get; set; }

        public decimal Amount { get; set; } 

        public int PupilId { get; set; }

        public Pupil Pupil { get; set; }

        public PaymentType Type { get; set; }

    }
}