using Mapster;
using MusicTutor.Core.Contracts.Pupils;
using MusicTutor.Core.Models;

namespace MusicTutor.Data.Mappings
{
    public class PupilMapping : IRegister
    {        
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Pupil, PupilResponseDto>()
                .Map(dest => dest.LessonRate, src => src.CurrentLessonRate);
                            
        }
    }
}