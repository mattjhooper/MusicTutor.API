using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicTutor.Core.Models;

namespace MusicTutor.Data.Configurations
{
    // https://docs.microsoft.com/en-us/ef/core/modeling/#use-fluent-api-to-configure-a-model
    public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            builder
                .Property(l => l.Cost)
                .HasPrecision(Lesson.DefaultPrecision,Lesson.DefaultScale)
                .IsRequired();                                 
        }
    }
}