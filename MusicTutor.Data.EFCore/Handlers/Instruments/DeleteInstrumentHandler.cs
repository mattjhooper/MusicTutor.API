using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MusicTutor.Core.Contracts.Instruments;
using MusicTutor.Cqs.Commands.Instruments;
using MusicTutor.Cqs.Queries.Instruments;

namespace MusicTutor.Data.EFCore.Handlers.Instruments
{
    public record DeleteInstrumentHandler(MusicTutorDbContext DbContext) : IRequestHandler<DeleteInstrument, int>
    {        
        public Task<int> Handle(DeleteInstrument request, CancellationToken cancellationToken)
        {
            int deletedCount = 0;
            var instrument = DbContext.Instruments.SingleOrDefault(i => i.Id == request.Id);

            if (instrument is not null)
            {
                DbContext.Remove(instrument);
                deletedCount++;
                DbContext.SaveChanges();
            }

            return Task.FromResult(deletedCount);
        }
    }
}
    