using Microsoft.EntityFrameworkCore;
using MusicTutor.Core.Models;
using System.Threading;
using System.Threading.Tasks;

namespace MusicTutor.Core.Services
{
    public interface IMusicTutorDbContext

    {
        DbSet<Pupil> Pupils  { get; set; } 
        DbSet<Instrument> Instruments  { get; set; } 

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}