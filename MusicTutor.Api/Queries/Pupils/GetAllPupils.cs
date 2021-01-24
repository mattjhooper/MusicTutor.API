using System;
using System.Collections.Generic;
using MediatR;
using MusicTutor.Api.Contracts.Pupils;

namespace MusicTutor.Api.Queries.Pupils
{
    public record GetAllPupils() : IRequest<IEnumerable<PupilResponseDto>> {}     

}