using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MusicTutor.Core.Services;
using MusicTutor.Data.EFCore;

namespace MusicTutor.IntegrationTests
{
    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup: class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<MusicTutorDbContext>));

                services.Remove(descriptor);

                services.AddDbContext<MusicTutorDbContext>(options =>
                {
                    options.UseSqlServer(@"server=.\SQLEXPRESS; database=MusicTutorIntTest; Trusted_Connection=True");
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<MusicTutorDbContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                    //db.Database.EnsureCreated();

                    try
                    {
                        var dataService = scopedServices.GetRequiredService<IDataService>();
                        dataService.RemoveDataStore();
                        dataService.SetupDataStore();
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the " +
                            "database with test messages. Error: {Message}", ex.Message);
                    }
                }
            });
        }
    }
}