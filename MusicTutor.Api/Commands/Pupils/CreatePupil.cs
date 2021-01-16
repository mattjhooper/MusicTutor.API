using MediatR;
using MusicTutor.Api.Contracts.Pupils;

namespace MusicTutor.Api.Commands.Pupils
{
    public record CreatePupil(CreatePupilDto PupilToCreate) : IRequest<PupilResponseDto> {}     

}