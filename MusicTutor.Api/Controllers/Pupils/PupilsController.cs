using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Http;
using MusicTutor.Api.Contracts.Errors;
using MusicTutor.Api.Contracts.Pupils;
using MusicTutor.Api.Queries.Pupils;
using MusicTutor.Api.Commands.Pupils;
using System.Collections.Generic;
using MusicTutor.Api.Commands.Auth;

namespace MusicTutor.Api.Controllers.Pupils
{
    public class PupilsController : AuthApiController
    {
        public PupilsController(IMediator mediator) : base(mediator)
        {
        }


        /// <summary>
        /// Gets all Pupils
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PupilResponseDto>>> GetManyAsync()
        {
            Console.WriteLine($"UserID: {UserId}");

            var pupils = await mediator.Send(new GetAllPupils());

            return Ok(pupils);
        }


        /// <summary>
        /// Gets the Pupil with the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetSinglePupil")]
        [ProducesResponseType(typeof(PupilResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PupilResponseDto>> GetSingleAsync([FromRoute] Guid id)
        {
            var pupil = await mediator.Send(new GetPupilById(id));

            if (pupil is null)
                return NotFound();

            return Ok(pupil);
        }

        /// <summary>
        /// Creates a new Pupil and returns the created entity, with the Id value provided by the database
        /// </summary>
        /// <remarks>
        /// Section to add any remarks
        /// </remarks>
        /// <param name="createPupil"></param>
        /// <returns>If successful it returns a CreatedAtRoute response - see
        /// https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-2.1#implement-the-other-crud-operations
        /// </returns>
        [ProducesResponseType(typeof(PupilResponseDto), StatusCodes.Status201Created)] //You need this, otherwise Swagger says the success status is 200, not 201
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<PupilResponseDto>> PostAsync([FromBody] CreatePupil createPupil)
        {
            try
            {
                var result = await mediator.Send(createPupil);
                //NOTE: to get this to work you MUST set the name of the HttpGet, e.g. [HttpGet("{id}", Name= "GetSinglePupil")],
                //on the Get you want to call, then then use the Name value in the Response.
                //Otherwise you get a "No route matches the supplied values" error.
                //see https://stackoverflow.com/questions/36560239/asp-net-core-createdatroute-failure for more on this
                return CreatedAtRoute("GetSinglePupil", new { id = result.Id }, result);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
        }

        /// <summary>
        /// Updates the supplied Pupil and returns the created entity
        /// </summary>
        /// <remarks>
        /// Section to add any remarks
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <returns>If successful it returns a CreatedAtRoute response - see
        /// https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-2.1#implement-the-other-crud-operations
        /// </returns>
        [ProducesResponseType(typeof(PupilResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [HttpPut("{id}")]
        public async Task<ActionResult<PupilResponseDto>> PutSingleAsync([FromRoute] Guid id, [FromBody] UpdatePupil item)
        {
            if (id != item.Id)
                return BadRequest($"Route Id [{id}] must match message Body Id [{item.Id}].");

            try
            {
                var pupil = await mediator.Send(item);

                if (pupil is null)
                    return NotFound();

                return Ok(pupil);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete the Pupil
        /// </summary>
        /// <returns></returns>
        // DELETE api/<type>/5
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItemAsync([FromRoute] Guid id)
        {
            var deleteCount = await mediator.Send(new DeletePupil(id));

            if (deleteCount == 0)
                return NotFound();

            return NoContent();
        }
    }
}