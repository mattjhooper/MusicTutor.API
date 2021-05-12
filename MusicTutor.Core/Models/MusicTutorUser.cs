using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using MusicTutor.Core.Models;

// https://docs.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-5.0#customize-the-model

public class MusicTutorUser : IdentityUser<Guid>
{
    private HashSet<Pupil> _pupils;
    public IEnumerable<Pupil> Pupils => _pupils;
}