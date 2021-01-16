using System;
using Mapster;
using MusicTutor.Core.Models;
using MusicTutor.Data.Mappings;

namespace MusicTutor.Api.Contracts.Pupils
{
    public record PupilResponseDto(Guid Id, string Name, decimal LessonRate, DateTime StartDate, int FrequencyInDays, decimal AccountBalance, string ContactName, string ContactEmail, string ContactPhoneNumber);
    
}