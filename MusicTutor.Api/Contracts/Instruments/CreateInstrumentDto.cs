using System.ComponentModel.DataAnnotations;
using MusicTutor.Core.Models;

namespace MusicTutor.Api.Contracts.Instruments
{
    public record CreateInstrumentDto(string Name);
}