using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MusicTutor.Exe
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Matt!");
            using (var context = new PupilsContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Add(new Pupil
                {
                    Name = "Matt",
                    Lessons = 
                    {
                        new Lesson
                        {
                            StartDateTime = DateTime.Now,
                            EndDateTime = DateTime.Now.AddHours(1),
                            Cost = 15M
                        },
                        new Lesson
                        {
                            StartDateTime = DateTime.Now.AddDays(7),
                            EndDateTime = DateTime.Now.AddDays(7).AddHours(1),
                            Cost = 15M
                        },
                        new Lesson
                        {
                            StartDateTime = DateTime.Now.AddDays(14),
                            EndDateTime = DateTime.Now.AddDays(14).AddHours(1),
                            Cost = 15M
                        }
                    }
                });

                context.SaveChanges();
            }

            using (var context = new PupilsContext())
            {
                var queryable = context.Pupils.Include(x => x.Lessons);

                var pupils = queryable.ToList();

                Console.WriteLine();
                Console.WriteLine();

                foreach (var pupil in pupils)
                {
                    Console.WriteLine($"Pupil: {pupil.Name}");
                    foreach (var lesson in pupil.Lessons)
                    {
                        Console.WriteLine($"   Lesson: {lesson.StartDateTime}");
                    }
                }
            }
        }
    }

    public class Pupil
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public ICollection<Lesson> Lessons { get; } = new List<Lesson>();
    }

    public class Lesson 
    {
        public int Id { get; set; }
        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public decimal Cost { get; set; } 

        public Pupil Pupil { get; set; }

    }

    public class PupilsContext : DbContext
    {
        public DbSet<Pupil> Pupils  { get; set; } 
        public DbSet<Lesson> Lessons  { get; set; } 

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .UseSqlServer(@"server=.\SQLEXPRESS; database=MusicTutor2020; user id=sa; password=Sql#2020");
    }
}
