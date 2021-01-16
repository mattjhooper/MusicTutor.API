using System;
using MediatR;
using MusicTutor.Api.Contracts.Pupils;

namespace MusicTutor.Api.Queries.Pupils
{
    public record GetPupilById(Guid Id) : IRequest<PupilResponseDto> {}     

}