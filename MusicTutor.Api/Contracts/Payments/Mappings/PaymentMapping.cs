using Mapster;
using MusicTutor.Core.Models;

namespace MusicTutor.Api.Contracts.Payments.Mappings
{
    public class PaymentMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Payment, PaymentResponseDto>();
        }
    }
}