using System;
using System.Threading;
using System.Threading.Tasks;

namespace MusicTutor.Api.Validators.Pupils
{
    public interface IDbValidator
    {
        Task<bool> InstrumentAlreadyExistsAsync(Guid instrumentId, CancellationToken cancellationToken);        
    }
}