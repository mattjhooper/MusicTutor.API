using System.ComponentModel.DataAnnotations;

namespace MusicTutor.Api.Controllers.Instruments.Dtos
{
    public class CreateInstrumentDto
    {
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }        
        
    }
}