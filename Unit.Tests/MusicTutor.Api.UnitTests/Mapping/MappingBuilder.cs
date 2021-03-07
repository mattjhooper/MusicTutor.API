using Mapster;
using MapsterMapper;
using MusicTutor.Api.Contracts.Instruments.Mappings;
using MusicTutor.Api.Contracts.Lessons.Mappings;
using MusicTutor.Api.Contracts.Pupils.Mappings;

namespace MusicTutor.Api.UnitTests.Mapping
{
    public class MappingBuilder
    {
        private readonly IMapper _mapper;
        private MappingBuilder()
        {
            var config = new TypeAdapterConfig();
            var pupilMapping = new PupilMapping();
            pupilMapping.Register(config);
            var instrumentMapping = new InstrumentMapping();
            instrumentMapping.Register(config);
            var lessonMapping = new LessonMapping();
            lessonMapping.Register(config);

            _mapper = new Mapper(config);
        }

        public static MappingBuilder Init()
        {
            return new MappingBuilder();
        }

        public IMapper Build()
        {
            return _mapper;
        }
    }
}