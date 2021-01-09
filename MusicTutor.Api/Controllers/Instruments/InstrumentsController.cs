using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicTutor.Core.Contracts.Instruments;
using MusicTutor.Cqs.Queries.Instruments;
using MusicTutor.Cqs.Commands.Instruments;
using System;
using Microsoft.AspNetCore.Http;
using MusicTutor.Core.Contracts.Errors;

namespace MusicTutor.Api.Controllers.Instruments
{
    public class InstrumentsController : BaseApiController
    {
        public InstrumentsController(IMediator mediator) : base(mediator)
        {                       
        }

        /// <summary>
        /// Gets all Instruments
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InstrumentResponseDto>>> GetManyAsync()
        {
            var instruments = await mediator.Send(new GetAllInstruments());
            
            return Ok(instruments);
        }

        /// <summary>
        /// Gets the Instrument with the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetSingleInstrument")]
        [ProducesResponseType(typeof(InstrumentResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<InstrumentResponseDto>> GetSingleAsync([FromRoute] Guid id)
        {
            var instrument = await mediator.Send(new GetByInstrumentId(id));

            if (instrument is null)
                return NotFound();

            return Ok(instrument);
        }

        /// <summary>
        /// Creates a new Instrument and returns the created entity, with the Id value provided by the database
        /// </summary>
        /// <remarks>
        /// Section to add any remarks
        /// </remarks>
        /// <param name="item"></param>
        /// <returns>If successful it returns a CreatedAtRoute response - see
        /// https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-2.1#implement-the-other-crud-operations
        /// </returns>
        [ProducesResponseType(typeof(InstrumentResponseDto), StatusCodes.Status201Created)] //You need this, otherwise Swagger says the success status is 200, not 201
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<InstrumentResponseDto>> PostAsync([FromBody] CreateInstrumentDto item)
        {
            try
            {
                var result = await mediator.Send(new CreateInstrument(item));
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

        // /// <summary>
        // /// Updates the Name. 
        // /// </summary>
        // /// <param name="dto">dto containing Id and Name</param>
        // [Route("name")]
        // [HttpPatch()]
        // public async Task<ActionResult<WebApiMessageOnly>> Name(CreateInstrumentDto dto)
        // {
        //     await _service.UpdateAndSaveAsync(dto);
        //     return _service.Response();
        // }

        /// <summary>
        /// Delete the Instrument
        /// </summary>
        /// <returns></returns>
        // DELETE api/<type>/5
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItemAsync([FromRoute] Guid id)
        {
            var instrument = await mediator.Send(new DeleteInstrument(id));

            if (instrument == 0)
                return NotFound();

            return NoContent();
        }
    }
}