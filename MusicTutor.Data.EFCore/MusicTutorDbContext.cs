using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;
using MusicTutor.Data.EFCore.Configuration;
using MusicTutor.Core.Models;
using MusicTutor.Core.Services;

namespace MusicTutor.Data.EFCore
{    
    public class MusicTutorDbContext : DbContext, IMusicTutorDbContext
    {
        public DbSet<Pupil> Pupils  { get; set; } 
        public DbSet<Instrument> Instruments  { get; set; } 

        public MusicTutorDbContext(DbContextOptions<MusicTutorDbContext> options)
            : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }                
    }
}