using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MusicTutor.Api.Commands.Pupils;
using MusicTutor.Core.Services;
using MusicTutor.Api.Commands.Auth;

namespace MusicTutor.Api.EFCore.Handlers.Pupils
{
    public record DeletePupilInstrumentLinkHandler(IMusicTutorDbContext DbContext) : IRequestHandler<WithMusicTutorUserId<DeletePupilInstrumentLink, int>, int>
    {
        public async Task<int> Handle(WithMusicTutorUserId<DeletePupilInstrumentLink, int> request, CancellationToken cancellationToken)
        {
            var deletePupilInstrumentLink = request.Request;
            var pupil = await DbContext.GetPupilWithInstrumentsForUserAsync(deletePupilInstrumentLink.pupilId, request.MusicTutorUserId);
            if (pupil is null)
                return -1;

            var deleteCount = pupil.RemoveInstrument(deletePupilInstrumentLink.instrumentId);

            if (deleteCount > 0)
                await DbContext.SaveChangesAsync(cancellationToken);

            return deleteCount;
        }
    }
}