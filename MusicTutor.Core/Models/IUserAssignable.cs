using System;

namespace MusicTutor.Core.Models
{
    public interface IUserAssignable
    {
        Guid MusicTutorUserId { get; }
        MusicTutorUser MusicTutorUser { get; }
        void AssignToMusicTutorUser(Guid musicTutorUserId);
    }
}