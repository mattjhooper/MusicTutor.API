using System;
using MediatR;
using MusicTutor.Api.Contracts.Pupils;
using MusicTutor.Core.Models;

namespace MusicTutor.Api.Commands.Pupils
{
    public record CreatePupil(string Name, decimal LessonRate, DateTime StartDate, int FrequencyInDays, Guid DefaultInstrumentId, string ContactName, string ContactEmail, string ContactPhoneNumber) : IRequest<PupilResponseDto> {}     

    public static class PupilCreator 
    {
        public static Pupil MakePupil(this CreatePupil p, Instrument instrument)
        {
            return new Pupil (p.Name, p.LessonRate, p.StartDate, p.FrequencyInDays, new Instrument[] {instrument}, p.ContactName, p.ContactEmail, p.ContactPhoneNumber);
        }
    }

}