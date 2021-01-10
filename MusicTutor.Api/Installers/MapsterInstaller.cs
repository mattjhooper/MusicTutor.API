using System.Reflection;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicTutor.Data.Mappings;

namespace MusicTutor.Api.Installers
{
    public class MapsterInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            TypeAdapterConfig.GlobalSettings.RequireExplicitMapping = true;
            //TypeAdapterConfig.GlobalSettings.RequireDestinationMemberSource = true;

            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetAssembly(typeof(PupilMapping)));

            TypeAdapterConfig.GlobalSettings.Compile();

            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();
        }
    }
}