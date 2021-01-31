using System;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using MusicTutor.Api;
using MusicTutor.Core.Models;
using Xunit;
using Microsoft.AspNetCore.TestHost;

namespace MusicTutor.IntegrationTests
{
    public abstract class IntegrationBase
    {
        protected const string InstrumentsUri = "/Instruments";
        protected const string PupilsUri = "/Pupils";
        protected HttpClient _client;
        public IntegrationBase()
        {
            var appFactory = new WebApplicationFactory<Startup>();
            _client = appFactory.CreateClient();
            _client.BaseAddress = new Uri("http://localhost:5001");
        }                
    }
}
