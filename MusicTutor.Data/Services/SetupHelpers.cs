using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using MusicTutor.Core.Models;
using MusicTutor.Data;

namespace MusicTutor.Data.Services
{
    public static class SetupHelpers
    {
        public const string SeedFileSubDirectory = "SeedData";

        public static void DevelopmentEnsureDeleted(this MusicTutorDbContext db)
        {
            db.Database.EnsureDeleted();
        }

        public static void DevelopmentEnsureCreated(this MusicTutorDbContext db)
        {
            db.Database.EnsureCreated();
        }
        
        public static int SeedDatabase(this MusicTutorDbContext context, string dataDirectory)
        {
            // if (!(context.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists())
            //     throw new InvalidOperationException("The database does not exist. If you are using Migrations then run PMC command update-database to create it");

            var numInstruments = context.Instruments.Count();
            if (numInstruments == 0)
            {
                //the database is empty so we fill it from a json file
                
                var seedDir = Path.Combine(dataDirectory, SeedFileSubDirectory);

                context.SeedInstruments(Path.Combine(seedDir, "instruments.json"));
                context.SaveChanges();
            
            }

            return numInstruments;
        }

        private static void SeedInstruments(this MusicTutorDbContext context, string dataPath)
        {
            var jsonString = File.ReadAllText(dataPath);
            var instruments = JsonSerializer.Deserialize<Instrument[]>(jsonString);

            context.Instruments.AddRange(instruments);
        }        
    }
}