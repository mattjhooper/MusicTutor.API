using System;

namespace MusicTutor.Core.Contracts.Pupils
{
    public record CreatePupilDto(string Name, decimal LessonRate, DateTime StartDate, int FrequencyInDays, string DefaultInstrument);
}

