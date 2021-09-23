using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MusicTutor.Core.Services;
using MusicTutor.Api.Commands.Pupils;

namespace MusicTutor.Api.EFCore.Handlers.Pupils
{
    public record DeletePupilHandler(IMusicTutorDbContext DbContext) : IRequestHandler<DeletePupil, int>
    {
        public async Task<int> Handle(DeletePupil request, CancellationToken cancellationToken)
        {
            var pupil = await DbContext.GetPupilAsync(request.Id);
            return await DeletePupil(pupil, cancellationToken);         
        }

        private async Task<int> DeletePupil(Core.Models.Pupil pupil, CancellationToken cancellationToken)
        {
            if (pupil is not null)
            {
                DbContext.Pupils.Remove(pupil);
                return await DbContext.SaveChangesAsync(cancellationToken);
            }

            return 0;
        }
    }
}
