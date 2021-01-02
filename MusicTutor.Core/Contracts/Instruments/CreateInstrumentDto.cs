using System.ComponentModel.DataAnnotations;
using MusicTutor.Core.Models;

namespace MusicTutor.Core.Contracts.Instruments
{
    public class CreateInstrumentDto
    {
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        public Instrument MapToInstrument()
        {
           return new Instrument(Name);
        }        
        
    }
}