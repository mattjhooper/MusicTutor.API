using System;
using System.Threading;
using System.Threading.Tasks;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MusicTutor.Api.Commands.Pupils;
using MusicTutor.Core.Services;
using MusicTutor.Api.Contracts.Instruments;

namespace MusicTutor.Api.EFCore.Handlers.Pupils
{
    public record DeletePupilInstrumentLinkHandler(IMusicTutorDbContext DbContext) : IRequestHandler<DeletePupilInstrumentLink, int>
    {        
        public async Task<int> Handle(DeletePupilInstrumentLink deletePupilInstrumentLink, CancellationToken cancellationToken)
        {
            var pupil = await DbContext.Pupils.Include(p => p.Instruments).SingleOrDefaultAsync(p => p.Id == deletePupilInstrumentLink.pupilId);
            if (pupil is null)                
                return -1;

            var deleteCount = pupil.RemoveInstrument(deletePupilInstrumentLink.instrumentId);

            if (deleteCount > 0)
                await DbContext.SaveChangesAsync(cancellationToken);

            return deleteCount;
        }
    }
}