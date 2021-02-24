using System;

namespace MusicTutor.Api.Contracts.Lessons
{
    public record LessonResponseDto(Guid Id, DateTime StartDateTime, int DurationInMinutes, decimal Cost, bool IsPlanned, string Status, Guid? InstrumentId, string InstrumentName);
}