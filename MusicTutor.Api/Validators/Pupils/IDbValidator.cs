using System;
using System.Threading;
using System.Threading.Tasks;
using MusicTutor.Api.Commands.Pupils;

namespace MusicTutor.Api.Validators.Pupils
{
    public interface IDbValidator
    {
        Task<bool> InstrumentAlreadyExistsAsync(Guid instrumentId, CancellationToken cancellationToken);        

        Task<bool> PupilAlreadyExistsAsync(Guid pupilId, CancellationToken cancellationToken);

        Task<bool> PupilInstrumentCanBeRemovedAsync(DeletePupilInstrumentLink deletePupilInstrumentLink, CancellationToken cancellationToken);
    }
}