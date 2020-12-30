using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;
using MusicTutor.Data.Configuration;
using MusicTutor.Data;
using MusicTutorAPI.Core.Models;

namespace MusicTutor.Exe
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new PupilsContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var pupil = new Pupil("Matt", 15.0M, DateTime.Now, 7, new Contact("Janet", "janet@mailinator.com"));
                pupil.AddLesson(new Lesson
                        {
                            StartDateTime = DateTime.Now,
                            EndDateTime = DateTime.Now.AddHours(1),
                            Cost = 15M
                        });

                pupil.AddLesson(new Lesson
                        {
                            StartDateTime = DateTime.Now.AddDays(7),
                            EndDateTime = DateTime.Now.AddDays(7).AddHours(1),
                            Cost = 15M
                        });  

                pupil.AddLesson(new Lesson
                        {
                            StartDateTime = DateTime.Now.AddDays(14),
                            EndDateTime = DateTime.Now.AddDays(14).AddHours(1),
                            Cost = 15M
                        });                                             


                context.Add(pupil);

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
}
