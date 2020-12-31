using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MusicTutor.Api;
using MusicTutor.Data;
using MusicTutor.Data.Services;

namespace MusicTutor.Api.Helpers
{
    public static class DatabaseStartupHelpers
    {

        public static IHost SetupDevelopmentDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                using (var context = services.GetRequiredService<MusicTutorDbContext>())
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();                        
                    try
                    {
                        logger.LogInformation("****** Call DevelopmentEnsureDeleted **********");
                        context.DevelopmentEnsureDeleted();
                        logger.LogInformation("****** Call DevelopmentEnsureCreated **********");
                        context.DevelopmentEnsureCreated();
                        logger.LogInformation("****** Call SeedDatabase **********");
                        context.SeedDatabase(Directory.GetCurrentDirectory());
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred while setting up or seeding the development database.");
                    }
                }
            }

            return host;
        }
    }
}