using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;
using MusicTutor.Data.Configuration;
using MusicTutor.Data;
using MusicTutor.Core.Models;

namespace MusicTutor.Exe
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new DbContextOptions<MusicTutorDbContext>();

            using (var context = new MusicTutorDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                Instrument piano = new ("Piano");
                Instrument flute = new ("Flute");
                
                Instrument[] instruments = { piano, flute }; 

                var pupil = new Pupil("Matt", 15.0M, DateTime.Now, 7, instruments, new Contact("Janet", "janet@mailinator.com"));
                DateTime completedLessonStart = DateTime.Now.AddHours(-1);
                pupil.AddCompletedLesson(completedLessonStart, 30, pupil.CurrentLessonRate);
                pupil.AddPlannedLesson(completedLessonStart.AddDays(pupil.FrequencyInDays), 30, pupil.CurrentLessonRate);    

                context.Add(pupil);

                DateTime pupil2Start = DateTime.Now.AddDays(3);
                var pupil2 = Pupil.CreatePupil("John", 15.0M,pupil2Start,7,new Instrument[] { new Instrument("Drums") }, "John Snr", null, "07890 123456" );
                pupil2.AddPlannedLesson(pupil2Start, 30, pupil2.CurrentLessonRate);
                context.Add(pupil2);

                var pupil3 = Pupil.CreatePupil("Sarah", 15.0M,DateTime.Now,7,new Instrument[] { piano }, "Carol", "carol@mailinator.com", "07890 123457" );
                pupil2.AddPlannedLesson(pupil2Start, 30, pupil2.CurrentLessonRate);
                context.Add(pupil3);

                context.Instruments.AddRange(instruments);

                context.SaveChanges();
            }

            using (var context = new MusicTutorDbContext(options))
            {
                var queryable = context.Pupils.Include(x => x.Lessons).Include(p => p.Instruments);

                var pupils = queryable.ToList();

                Console.WriteLine();
                Console.WriteLine();

                foreach (var pupil in pupils)
                {
                    Console.WriteLine($"Pupil: {pupil.Name}. Account Balance: {pupil.AccountBalance}. Instruments: {pupil.InstrumentsToString()}");
                    foreach (var lesson in pupil.Lessons)
                    {
                        Console.WriteLine($"   Lesson: {lesson.StartDateTime}. {lesson.Status}");
                    }
                }

                Console.WriteLine();
                Console.WriteLine();

                var instruments = context.Instruments.Include(i => i.Pupils).ToList();

                foreach(var instrument in instruments)
                {
                    Console.WriteLine($"Instrument: {instrument.Name}");
                    instrument.Pupils.ToList().ForEach(p => Console.WriteLine("   " + p.Name));
                }
            }
        }
    }    
}
