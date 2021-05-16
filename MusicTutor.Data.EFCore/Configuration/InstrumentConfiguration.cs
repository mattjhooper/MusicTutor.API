using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicTutor.Core.Models;


namespace MusicTutor.Data.EFCore.Configuration
{
    public class InstrumentConfiguration : IEntityTypeConfiguration<Instrument>
    {
        public void Configure(EntityTypeBuilder<Instrument> builder)
        {
            builder
                .Property(i => i.Name)
                .IsRequired()
                .HasMaxLength(Instrument.NameLength);

            builder
                .HasIndex(i => new { i.MusicTutorUserId, i.Name })
                .IsUnique();
        }
    }
}