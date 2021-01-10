using System.Threading;
using System.Threading.Tasks;
using Mapster;
using MapsterMapper;
using MediatR;
using MusicTutor.Core.Contracts.Pupils;
using MusicTutor.Core.Models;
using MusicTutor.Cqs.Commands.Pupils;

namespace MusicTutor.Data.EFCore.Handlers.Instruments
{
    public record CreatePupilHandler(MusicTutorDbContext DbContext, IMapper Mapper) : IRequestHandler<CreatePupil, PupilResponseDto>
    {        
        public async Task<PupilResponseDto> Handle(CreatePupil request, CancellationToken cancellationToken)
        {
            var pupil = Mapper.Map<Pupil>(request.PupilToCreate);
            await DbContext.AddAsync<Pupil>(pupil);
            await DbContext.SaveChangesAsync();

            return Mapper.Map<PupilResponseDto>(pupil);
        }
    }
}
    