using System.ComponentModel.DataAnnotations;
using MusicTutor.Core.Models;

namespace MusicTutor.Core.Contracts.Instruments
{
    public enum InstrumentType
    {
        Wind,
        Percussion,
        String
    }
    public record CreateInstrumentDto(string Name, string InstrumentType)
    {
        public Instrument MapToInstrument()
        {
           return new Instrument(Name);
        }        
        
    }
}