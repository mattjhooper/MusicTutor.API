using MapsterMapper;
using MusicTutor.Api.UnitTests.Mapping;
using MusicTutor.Api.EFCore.Handlers.Pupils;
using MusicTutor.Core.Services;
using MusicTutor.Api.UnitTests.Utils;
using Xunit;
using MusicTutor.Api.Commands.Pupils;
using System.Threading.Tasks;
using System.Threading;
using FluentAssertions;
using NSubstitute;
using MusicTutor.Core.Models;
using System.Linq;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace MusicTutor.Api.UnitTests
{
    public class StartupUnitTests
    {
        public StartupUnitTests()
        {
        }

        [Fact]
        public void Startup_ConfigureServicesRunsOK()
        {
            var config = Substitute.For<IConfiguration>();
            var service = Substitute.For<IServiceCollection>();
            //Given
            var startup = new Startup(config);

            startup.ConfigureServices(service);
        }

        // [Fact]
        // public void Startup_ConfigureRunsOK()
        // {
        //     var config = Substitute.For<IConfiguration>();
        //     var app = Substitute.For<IApplicationBuilder>();
        //     var env = Substitute.For<IWebHostEnvironment>();
            

        //     //Given
        //     var startup = new Startup(config);

        //     startup.Configure(app, env);
        // }

        
   }
}