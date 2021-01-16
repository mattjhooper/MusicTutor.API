using Mapster;
using MusicTutor.Api.Contracts.Instruments;
using MusicTutor.Core.Models;

namespace MusicTutor.Api.Contracts.Instruments.Mappings
{
    public class InstrumentMapping : IRegister
    {        
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Instrument, InstrumentResponseDto>();                            
        }
    }
} 