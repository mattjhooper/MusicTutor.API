using System;
using MediatR;
using MusicTutor.Api.Contracts.Pupils;
using MusicTutor.Core.Models;

namespace MusicTutor.Api.Commands.Pupils
{
    public record UpdatePupil(Guid Id, string Name, decimal LessonRate, DateTime StartDate, int FrequencyInDays, string ContactName, string ContactEmail, string ContactPhoneNumber) : IRequest<PupilResponseDto> { };

}