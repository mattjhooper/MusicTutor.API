using System;
using System.Net.Http;
using MusicTutor.Api.Controllers;
using Xunit;

namespace MusicTutor.IntegrationTests
{
    // https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-5.0
    public abstract class IntegrationBase: IClassFixture<CustomWebApplicationFactory<MusicTutor.Api.Startup>>
    {
        protected const string InstrumentsUri = "/" + BaseApiController.Instruments;
        protected const string PupilsUri = "/" + BaseApiController.Pupils;
        protected readonly CustomWebApplicationFactory<MusicTutor.Api.Startup> _factory;
        
        protected HttpClient _client;
        public IntegrationBase(CustomWebApplicationFactory<MusicTutor.Api.Startup> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
            _client.BaseAddress = new Uri("http://localhost:5001");                        
        }              
    }
}
