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
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MusicTutor.Services.Auth;

namespace MusicTutor.Data.EFCore
{
    public class MusicTutorDbContext : IdentityDbContext<MusicTutorUser, IdentityRole<Guid>, Guid>, IMusicTutorDbContext
    {
        public DbSet<Pupil> Pupils { get; set; }
        public DbSet<Instrument> Instruments { get; set; }

        public Guid CurrentUserId => _userRepository.UserId;

        private readonly IUserRepository _userRepository;

        public MusicTutorDbContext(DbContextOptions<MusicTutorDbContext> options, IUserRepository userRepository)
            : base(options)
        {
            _userRepository = userRepository;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            builder.Entity<Instrument>().HasQueryFilter(i => i.MusicTutorUserId == CurrentUserId);

        }
    }
}