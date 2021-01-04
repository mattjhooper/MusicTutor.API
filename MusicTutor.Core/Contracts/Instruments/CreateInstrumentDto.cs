using System.ComponentModel.DataAnnotations;
using MusicTutor.Core.Models;

namespace MusicTutor.Core.Contracts.Instruments
{
    public record CreateInstrumentDto(string Name)
    {
        public Instrument MapToInstrument()
        {
           return new Instrument(Name);
        }        
        
    }
}