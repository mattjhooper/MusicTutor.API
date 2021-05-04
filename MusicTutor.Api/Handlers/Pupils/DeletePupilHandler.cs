using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MusicTutor.Core.Services;
using MusicTutor.Api.Commands.Pupils;
using MusicTutor.Api.Commands.Auth;

namespace MusicTutor.Api.EFCore.Handlers.Pupils
{
    public record DeletePupilHandler(IMusicTutorDbContext DbContext) : IRequestHandler<WithMusicTutorUserId<DeletePupil, int>, int>
    {
        public async Task<int> Handle(WithMusicTutorUserId<DeletePupil, int> requestWithUserId, CancellationToken cancellationToken)
        {
            int deletedCount = 0;
            var Pupil = await DbContext.GetPupilForUserAsync(requestWithUserId.Request.Id, requestWithUserId.MusicTutorUserId);

            if (Pupil is not null)
            {
                DbContext.Pupils.Remove(Pupil);
                deletedCount = await DbContext.SaveChangesAsync(cancellationToken);
            }

            return deletedCount;
        }
    }
}
