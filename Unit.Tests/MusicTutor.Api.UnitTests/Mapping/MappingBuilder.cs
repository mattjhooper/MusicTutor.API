using Mapster;
using MapsterMapper;
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