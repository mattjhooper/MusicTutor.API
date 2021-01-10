using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicTutor.Cqs.Queries.Instruments;
using MusicTutor.Cqs.Commands.Instruments;
using System;
using Microsoft.AspNetCore.Http;
using MusicTutor.Core.Contracts.Errors;
using MusicTutor.Core.Contracts.Pupils;
using MusicTutor.Cqs.Queries.Pupils;
using MusicTutor.Cqs.Commands.Pupils;

namespace MusicTutor.Api.Controllers.Pupils
{
    public class PupilsController : BaseApiController
    {
        public PupilsController(IMediator mediator) : base(mediator)
        {                       
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
        /// <param name="item"></param>
        /// <returns>If successful it returns a CreatedAtRoute response - see
        /// https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-2.1#implement-the-other-crud-operations
        /// </returns>
        [ProducesResponseType(typeof(PupilResponseDto), StatusCodes.Status201Created)] //You need this, otherwise Swagger says the success status is 200, not 201
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<PupilResponseDto>> PostAsync([FromBody] CreatePupilDto item)
        {
            //var result = new PupilResponseDto(Guid.NewGuid(), item.Name, item.LessonRate, item.StartDate, item.FrequencyInDays, 0);
            //return CreatedAtRoute("GetSinglePupil", new { id = result.Id } , result);
            
            try
            {
                var result = await mediator.Send(new CreatePupil(item));
                //NOTE: to get this to work you MUST set the name of the HttpGet, e.g. [HttpGet("{id}", Name= "GetSinglePupil")],
                //on the Get you want to call, then then use the Name value in the Response.
                //Otherwise you get a "No route matches the supplied values" error.
                //see https://stackoverflow.com/questions/36560239/asp-net-core-createdatroute-failure for more on this
                return CreatedAtRoute("GetSinglePupil", new { id = result.Id } , result);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
        }        
    }
}