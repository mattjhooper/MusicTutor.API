using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MusicTutor.Api.Contracts.Instruments;
using MusicTutor.Core.Models;
using MusicTutor.Core.Services;
using MusicTutor.Api.Commands.Instruments;
using MusicTutor.Data.InMemory.Services;

namespace MusicTutor.Data.InMemory.Handlers
{
    public record BaseHandler
    {        
        public DataServiceInMemory DataService { get; init; }

        public BaseHandler(IDataService dataService)
        {
            if (dataService is DataServiceInMemory)
                DataService = (DataServiceInMemory)dataService;
            else
                DataService = new DataServiceInMemory();
        }        
    }
}
    