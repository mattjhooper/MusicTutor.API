using System;

namespace MusicTutor.Api.Contracts.Pupils
{
    public record CreatePupilDto(string Name, decimal LessonRate, DateTime StartDate, int FrequencyInDays, Guid DefaultInstrumentId, string ContactName, string ContactEmail, string ContactPhoneNumber);
}

