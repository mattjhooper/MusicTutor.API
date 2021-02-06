using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicTutor.Api.Contracts.Instruments;
using MusicTutor.Api.Queries.Pupils;

namespace MusicTutor.Api.Controllers.Pupils
{
    [Route(Pupils)]
    public class PupilInstrumentsController : BaseApiController
    {
        public PupilInstrumentsController(IMediator mediator) : base(mediator)
        {                       
        }

        /// <summary>
        /// Gets all Pupil Instruments
        /// </summary>
        /// <returns></returns>
        [HttpGet("{pupilId}/Instruments", Name = "GetPupilInstruments")]
        [ProducesResponseType(typeof(IEnumerable<InstrumentResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<InstrumentResponseDto>>> GetManyAsync([FromRoute] Guid pupilId)
        {
            var instruments = await mediator.Send(new GetPupilInstruments(pupilId));

            if (instruments is null)
                return NotFound();

            return Ok(instruments);
        }
    }
}