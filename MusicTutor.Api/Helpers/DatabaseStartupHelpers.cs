using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MusicTutor.Core.Services;

namespace MusicTutor.Api.Helpers
{
    public static class DatabaseStartupHelpers
    {

        public static IHost SetupDevelopmentDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var dataService = services.GetRequiredService<IDataService>();
                dataService.RemoveDataStore();
                dataService.SetupDataStore();
            }

            return host;
        }
    }
}