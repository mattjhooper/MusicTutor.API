using System;

namespace MusicTutor.Services.Auth
{
    public interface IUserRepository
    {
        Guid UserId { get; }
    }
}