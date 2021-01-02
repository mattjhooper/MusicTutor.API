using System.Reflection;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicTutor.Data.EFCore.Handlers.Instruments;

namespace MusicTutor.Api.Installers
{
    public class MediatRInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var dataAssembly = Assembly.GetAssembly(typeof(CreateInstrumentHandler));
            services.AddMediatR(Assembly.GetExecutingAssembly(), dataAssembly );
        }
    }
}