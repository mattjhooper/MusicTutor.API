using System.Threading;
using System.Threading.Tasks;
using MapsterMapper;
using MediatR;
using MusicTutor.Api.Contracts.Pupils;
using MusicTutor.Api.Commands.Pupils;
using MusicTutor.Core.Services;
using MusicTutor.Api.Commands.Auth;

namespace MusicTutor.Api.EFCore.Handlers.Pupils
{
    public record UpdatePupilHandler(IMusicTutorDbContext DbContext, IMapper Mapper) : IRequestHandler<UpdatePupil, PupilResponseDto>
    {
        public async Task<PupilResponseDto> Handle(UpdatePupil updatePupil, CancellationToken cancellationToken)
        {

            var pupil = await DbContext.GetPupilAsync(updatePupil.Id);

            if (pupil is null)
                return null;

            pupil.UpdatePupil(updatePupil.Name, updatePupil.LessonRate, updatePupil.StartDate, updatePupil.FrequencyInDays, updatePupil.ContactName, updatePupil.ContactEmail, updatePupil.ContactPhoneNumber);

            await DbContext.SaveChangesAsync(cancellationToken);

            return Mapper.Map<PupilResponseDto>(pupil);
        }
    }
}
