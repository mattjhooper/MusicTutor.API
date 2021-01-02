using System.ComponentModel.DataAnnotations;
using Ardalis.GuardClauses;
using MusicTutor.Core.Models;

namespace MusicTutor.Core.Contracts.Instruments
{
    public record InstrumentResponseDto(int Id, string Name)
    {
        public static InstrumentResponseDto MapFromInstrument(Instrument instrument)
        {
            Guard.Against.Null(instrument, nameof(instrument));
            return new InstrumentResponseDto(instrument.Id, instrument.Name);
        }
    }
}