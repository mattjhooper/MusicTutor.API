using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicTutor.Core.Models;


namespace MusicTutor.Data.EFCore.Configuration
{
    public class PupilConfiguration : IEntityTypeConfiguration<Pupil>
    {
        public void Configure(EntityTypeBuilder<Pupil> builder)
        {
            builder
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(Pupil.NameLength);

            builder
                .Property(p => p.AccountBalance)
                .HasPrecision(Pupil.DefaultPrecision, Pupil.DefaultScale)
                .IsRequired();

            builder
                .Property(p => p.CurrentLessonRate)
                .HasPrecision(Pupil.DefaultPrecision, Pupil.DefaultScale)
                .IsRequired();

            builder
                .Property(p => p.Timestamp)
                .IsRowVersion();

            builder
                .OwnsOne(
                    p => p.Contact,
                    c =>
                    {
                        c.Property(p => p.Name).IsRequired().HasMaxLength(Contact.NameLength);
                        c.Property(p => p.Email).HasMaxLength(Contact.EmailLength);
                        c.Property(p => p.PhoneNumber).HasMaxLength(Contact.PhoneNumberLength);
                    });

            builder
                .HasOne(p => p.MusicTutorUser)
                .WithMany(m => m.Pupils)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}