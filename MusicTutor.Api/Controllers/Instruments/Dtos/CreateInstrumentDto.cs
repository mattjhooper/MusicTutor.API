using System.ComponentModel.DataAnnotations;
using MusicTutor.Core.Models;

namespace MusicTutor.Api.Controllers.Instruments.Dtos
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