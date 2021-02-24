using Mapster;
using MusicTutor.Api.Contracts.Lessons;
using MusicTutor.Core.Models;

namespace MusicTutor.Api.Contracts.Lessons.Mappings
{
    public class LessonMapping : IRegister
    {        
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Lesson, LessonResponseDto>();                            
        }
    }
} 