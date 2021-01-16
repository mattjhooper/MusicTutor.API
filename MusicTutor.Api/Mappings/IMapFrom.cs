using Mapster;

namespace MusicTutor.Data.Mappings
{
    public interface IMapFrom<T>
    {   
        void Register(TypeAdapterConfig config) => config.NewConfig(typeof(T), GetType());
    }
}
