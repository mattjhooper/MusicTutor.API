using Mapster;
using MusicTutor.Api.Contracts.Pupils;
using MusicTutor.Core.Models;

namespace MusicTutor.Api.Contracts.Pupils.Mappings
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