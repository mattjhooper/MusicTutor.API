using Microsoft.EntityFrameworkCore;
using MusicTutor.Core.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MusicTutor.Core.Services
{
    public interface IMusicTutorDbContext

    {
        Guid CurrentUserId { get; }
        DbSet<Pupil> Pupils { get; set; }
        DbSet<Instrument> Instruments { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}