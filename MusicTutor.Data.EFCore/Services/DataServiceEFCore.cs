using System;
using System.IO;
using Microsoft.Extensions.Logging;
using MusicTutor.Core.Services;

namespace MusicTutor.Data.EFCore.Services
{
    public class DataServiceEFCore : IDataService
    {
        private readonly MusicTutorDbContext context;
        private readonly ILogger<DataServiceEFCore> logger;

        public DataServiceEFCore(ILogger<DataServiceEFCore> logger, MusicTutorDbContext context)
        {
            this.context = context;
            this.logger = logger;
        }

        public void SetupDataStore()
        {
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
}