using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MusicTutor.Core.Models;
using MusicTutor.Data;
using MusicTutor.Api.Controllers.Instruments.Dtos;

namespace MusicTutor.Api.Queries.Instruments
{
    public record GetByInstrumentId(int Id) : IRequest<InstrumentResponseDto> {}     

}