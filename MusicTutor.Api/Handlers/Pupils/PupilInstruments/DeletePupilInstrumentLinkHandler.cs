using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MusicTutor.Api.Commands.Pupils;
using MusicTutor.Core.Services;

namespace MusicTutor.Api.EFCore.Handlers.Pupils
{
    public record DeletePupilInstrumentLinkHandler(IMusicTutorDbContext DbContext) : IRequestHandler<DeletePupilInstrumentLink, int>
    {
        public async Task<int> Handle(DeletePupilInstrumentLink deletePupilInstrumentLink, CancellationToken cancellationToken)
        {
            var pupil = await DbContext.GetPupilWithInstrumentsAsync(deletePupilInstrumentLink.pupilId);
            if (pupil is null)
                return -1;

            var deleteCount = pupil.RemoveInstrument(deletePupilInstrumentLink.instrumentId);

            if (deleteCount > 0)
                await DbContext.SaveChangesAsync(cancellationToken);

            return deleteCount;
        }
    }
}