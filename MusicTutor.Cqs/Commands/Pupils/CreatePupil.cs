using MediatR;
using MusicTutor.Core.Contracts.Pupils;

namespace MusicTutor.Cqs.Commands.Pupils
{
    public record CreatePupil(CreatePupilDto PupilToCreate) : IRequest<PupilResponseDto> {}     

}