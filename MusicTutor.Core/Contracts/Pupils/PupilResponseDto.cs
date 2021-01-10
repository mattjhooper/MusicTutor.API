using System;

namespace MusicTutor.Core.Contracts.Pupils
{
    public record PupilResponseDto(Guid Id, string Name, decimal LessonRate, DateTime StartDate, int FrequencyInDays, decimal AccountBalance, string ContactName, string ContactEmail, string ContactPhoneNumber);
}