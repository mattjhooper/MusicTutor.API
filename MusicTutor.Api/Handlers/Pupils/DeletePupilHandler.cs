using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MusicTutor.Api.Contracts.Pupils;
using MusicTutor.Core.Services;
using MusicTutor.Api.Commands.Pupils;
using MusicTutor.Api.Queries.Pupils;

namespace MusicTutor.Api.EFCore.Handlers.Pupils
{
    public record DeletePupilHandler(IMusicTutorDbContext DbContext) : IRequestHandler<DeletePupil, int>
    {
        public async Task<int> Handle(DeletePupil request, CancellationToken cancellationToken)
        {
            int deletedCount = 0;
            var Pupil = DbContext.Pupils.SingleOrDefault(i => i.Id == request.Id);

            if (Pupil is not null)
            {
                DbContext.Pupils.Remove(Pupil);
                deletedCount = await DbContext.SaveChangesAsync(cancellationToken);
            }

            return deletedCount;
        }
    }
}
