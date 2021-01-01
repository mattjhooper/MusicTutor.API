using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MusicTutor.Core.Models;
using MusicTutor.Data;
using MusicTutor.Api.Controllers.Instruments.Dtos;
using System.Collections.Generic;

namespace MusicTutor.Api.Queries.Instruments
{
    public record GetAllInstruments : IRequest<IEnumerable<InstrumentResponseDto>> {}     

}