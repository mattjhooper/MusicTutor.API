using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicTutor.Api.Commands.Pupils;
using MusicTutor.Api.Contracts.Errors;
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

        /// <summary>
        /// Creates a new Pupil Instrument Link and returns the instrument
        /// </summary>
        /// <param name="pupilId"></param>
        /// <param name="item"></param>
        /// <returns>If successful it returns a CreatedAtRoute response - see
        /// https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-2.1#implement-the-other-crud-operations
        /// </returns>
        [ProducesResponseType(typeof(InstrumentResponseDto), StatusCodes.Status201Created)] //You need this, otherwise Swagger says the success status is 200, not 201
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [HttpPost("{pupilId}/Instruments", Name = "AddPupilInstrument")]
        public async Task<ActionResult<InstrumentResponseDto>> PostAsync([FromRoute] Guid pupilId, [FromBody] CreatePupilInstrumentLink item)
        {
            if (pupilId != item.pupilId)
                return BadRequest($"Route Id [{pupilId}] must match message Body Id [{item.pupilId}].");
            
            try
            {
                var result = await mediator.Send(item);

                if (result is null)
                    return NotFound();

                //NOTE: to get this to work you MUST set the name of the HttpGet, e.g. [HttpGet("{id}", Name= "GetSingleInstrument")],
                //on the Get you want to call, then then use the Name value in the Response.
                //Otherwise you get a "No route matches the supplied values" error.
                //see https://stackoverflow.com/questions/36560239/asp-net-core-createdatroute-failure for more on this
                return CreatedAtRoute("GetSingleInstrument", new { id = result.Id } , result);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
        }

         /// <summary>
        /// Creates a new Pupil Instrument Link and returns the instrument
        /// </summary>
        /// <param name="pupilId"></param>
        /// <param name="instrumentId"></param>
        /// <returns>If successful it returns a CreatedAtRoute response - see
        /// https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-2.1#implement-the-other-crud-operations
        /// </returns>
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [HttpDelete("{pupilId}/Instruments/{instrumentId}", Name = "DeletePupilInstrument")]
        public async Task<ActionResult> DeleteAsync([FromRoute] Guid pupilId, [FromRoute] Guid instrumentId)
        {
            try
            {
                var result = await mediator.Send(new DeletePupilInstrumentLink(pupilId, instrumentId));

                if (result <= 0)
                    return NotFound();

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
        }
    }
}