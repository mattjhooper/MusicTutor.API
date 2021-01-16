using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MusicTutor.Api.Contracts.Instruments;
using MusicTutor.Core.Services;
using MusicTutor.Api.Commands.Instruments;
using MusicTutor.Api.Queries.Instruments;
using MusicTutor.Data.InMemory.Services;

namespace MusicTutor.Data.InMemory.Handlers.Instruments
{
    public record DeleteInstrumentHandler: BaseHandler, IRequestHandler<DeleteInstrument, int>
    {        
        public DeleteInstrumentHandler(IDataService dataService) : base(dataService) {}

        public Task<int> Handle(DeleteInstrument request, CancellationToken cancellationToken)
        {
            int removedCount = DataService.Instruments.RemoveWhere(i => i.Id == request.Id);

            return Task.FromResult(removedCount);
        }
    }
}
    