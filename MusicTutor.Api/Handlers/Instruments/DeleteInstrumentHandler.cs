using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MusicTutor.Api.Contracts.Instruments;
using MusicTutor.Core.Services;
using MusicTutor.Api.Commands.Instruments;
using MusicTutor.Api.Queries.Instruments;

namespace MusicTutor.Api.EFCore.Handlers.Instruments
{
    public record DeleteInstrumentHandler(IMusicTutorDbContext DbContext) : IRequestHandler<DeleteInstrument, int>
    {        
        public async Task<int> Handle(DeleteInstrument request, CancellationToken cancellationToken)
        {
            int deletedCount = 0;
            var instrument = DbContext.Instruments.SingleOrDefault(i => i.Id == request.Id);

            if (instrument is not null)
            {
                DbContext.Instruments.Remove(instrument);
                deletedCount = await DbContext.SaveChangesAsync(cancellationToken);
            }

            return deletedCount;
        }
    }
}
    