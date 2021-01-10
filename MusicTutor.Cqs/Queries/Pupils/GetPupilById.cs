using System;
using MediatR;
using MusicTutor.Core.Contracts.Pupils;

namespace MusicTutor.Cqs.Queries.Pupils
{
    public record GetPupilById(Guid Id) : IRequest<PupilResponseDto> {}     

}