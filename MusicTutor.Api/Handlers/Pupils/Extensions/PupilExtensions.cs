using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicTutor.Core.Models;
using MusicTutor.Core.Services;

namespace MusicTutor.Api.EFCore.Handlers.Pupils
{
    public static class PupilExtensions
    {
        public static async Task<Pupil> GetPupilAsync(this IMusicTutorDbContext context, Guid pupilId)
        {
            return await context.Pupils.SingleOrDefaultAsync(p => p.Id == pupilId);
        }

        public static async Task<Pupil> GetPupilWithInstrumentsForUserAsync(this IMusicTutorDbContext context, Guid pupilId, Guid musicTutorUserId)
        {
            return await context.Pupils.Include(p => p.Instruments).SingleOrDefaultAsync(p => p.Id == pupilId && p.MusicTutorUserId == musicTutorUserId);
        }

        public static async Task<Pupil> GetPupilWithLessonsForUserAsync(this IMusicTutorDbContext context, Guid pupilId, Guid musicTutorUserId)
        {
            return await context.Pupils.Include(p => p.Lessons).SingleOrDefaultAsync(p => p.Id == pupilId && p.MusicTutorUserId == musicTutorUserId);
        }

        public static async Task<Pupil> GetPupilWithPaymentsForUserAsync(this IMusicTutorDbContext context, Guid pupilId, Guid musicTutorUserId)
        {
            return await context.Pupils.Include(p => p.Payments).SingleOrDefaultAsync(p => p.Id == pupilId && p.MusicTutorUserId == musicTutorUserId);
        }
    }
}