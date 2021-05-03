using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicTutor.Core.Models;
using MusicTutor.Core.Services;

namespace MusicTutor.Api.EFCore.Handlers.Pupils
{
    public static class PupilExtensions
    {
        public static async Task<Pupil> GetPupilForUserAsync(this IMusicTutorDbContext context, Guid pupilId, Guid musicTutorUserId)
        {
            return await context.Pupils.SingleOrDefaultAsync(p => p.Id == pupilId && p.MusicTutorUserId == musicTutorUserId);
        }

    }
}