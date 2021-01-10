using Mapster;
using MusicTutor.Core.Contracts.Pupils;
using MusicTutor.Core.Models;

namespace MusicTutor.Data.Mappings
{
    public class PupilMapping : IRegister
    {        
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CreatePupilDto, Pupil>()
                .ConstructUsing(src => new Pupil(src.Name, src.LessonRate, src.StartDate, src.FrequencyInDays, new Instrument[] { new Instrument(src.DefaultInstrument) }, null))
                .IgnoreNonMapped(true);

            config.NewConfig<Pupil, PupilResponseDto>()
                .ConstructUsing(src => new PupilResponseDto(src.Id, src.Name, src.CurrentLessonRate, src.StartDate, src.FrequencyInDays, src.AccountBalance))
                .IgnoreNonMapped(true);                
        }
    }
}