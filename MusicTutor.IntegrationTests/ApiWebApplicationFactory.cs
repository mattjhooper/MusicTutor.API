using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicTutor.Data.EFCore;
using MusicTutor.Data.EFCore.Services;

namespace MusicTutor.IntegrationTests
{
    public class ApiWebApplicationFactory : WebApplicationFactory<MusicTutor.Api.Startup>
    {
        private static readonly object _lock = new object();
        private static bool _databaseInitialized;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(config =>
            {
                var integrationConfig = new ConfigurationBuilder()
                  .AddJsonFile("appsettings.Integration.json")
                  .Build();

                config.AddConfiguration(integrationConfig);

                
            });

            builder.ConfigureServices(services =>
            {
                lock (_lock)
                {
                    if (! _databaseInitialized)
                    {
                        var sp = services.BuildServiceProvider();

                        using (var scope = sp.CreateScope())
                        {
                            var scopedServices = scope.ServiceProvider;
                            var db = scopedServices.GetRequiredService<MusicTutorDbContext>();
                            db.Database.EnsureCreated(); 
                            db.SeedDatabase(Directory.GetCurrentDirectory());                   

                            _databaseInitialized = true;
                        }
                    }
                }
            });
        }
    }
}