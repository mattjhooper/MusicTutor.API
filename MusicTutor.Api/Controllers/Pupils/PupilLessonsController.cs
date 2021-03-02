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
using MusicTutor.Api.Contracts.Lessons;

namespace MusicTutor.Api.Controllers.Pupils
{
    [Route(Pupils)]
    public class PupilLessonsController : BaseApiController
    {
        public PupilLessonsController(IMediator mediator) : base(mediator)
        {                       
        }


        /// <summary>
        /// Gets the PupilLesson with the given id
        /// </summary>
        /// <param name="pupilId"></param>
        /// <param name="lessonId"></param>
        /// <returns></returns>
        [HttpGet("{pupilId}/Lessons/{lessonId}", Name = "GetSinglePupilLesson")]
        [ProducesResponseType(typeof(LessonResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LessonResponseDto>> GetSingleAsync([FromRoute] Guid pupilId, [FromRoute] Guid lessonId)
        {
            var lesson = await mediator.Send(new GetPupilLessonById(pupilId, lessonId));

            if (lesson is null)
                return NotFound();

            return Ok(lesson);
        }

        /// <summary>
        /// Gets all Pupil Lessons
        /// </summary>
        /// <returns></returns>
        [HttpGet("{pupilId}/Lessons", Name = "GetPupilLessons")]
        [ProducesResponseType(typeof(IEnumerable<LessonResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<LessonResponseDto>>> GetManyAsync([FromRoute] Guid pupilId)
        {
            var lessons = await mediator.Send(new GetPupilLessons(pupilId));

            return Ok(lessons);
        }

        /// <summary>
        /// Creates a new Pupil Lesson Link and returns the Lesson
        /// </summary>
        /// <param name="pupilId"></param>
        /// <param name="item"></param>
        /// <returns>If successful it returns a CreatedAtRoute response - see
        /// https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-2.1#implement-the-other-crud-operations
        /// </returns>
        [ProducesResponseType(typeof(LessonResponseDto), StatusCodes.Status201Created)] //You need this, otherwise Swagger says the success status is 200, not 201
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [HttpPost("{pupilId}/Lessons", Name = "AddPupilLesson")]
        public async Task<ActionResult<LessonResponseDto>> PostAsync([FromRoute] Guid pupilId, [FromBody] CreatePupilLesson item)
        {
            if (pupilId != item.PupilId)
                return BadRequest($"Route Id [{pupilId}] must match message Body Id [{item.PupilId}].");
            
            try
            {
                var result = await mediator.Send(item);

                if (result is null)
                    return NotFound();

                //NOTE: to get this to work you MUST set the name of the HttpGet, e.g. [HttpGet("{id}", Name= "GetSingleLesson")],
                //on the Get you want to call, then then use the Name value in the Response.
                //Otherwise you get a "No route matches the supplied values" error.
                //see https://stackoverflow.com/questions/36560239/asp-net-core-createdatroute-failure for more on this
                return CreatedAtRoute("GetSinglePupilLesson", new { pupilId = item.PupilId, lessonId = result.Id } , result);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
        }

        /// <summary>
        /// Deletes the Pupil Lesson
        /// </summary>
        /// <param name="pupilId"></param>
        /// <param name="lessonId"></param>
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [HttpDelete("{pupilId}/lessons/{lessonId}", Name = "DeletePupilLesson")]
        public async Task<ActionResult> DeleteAsync([FromRoute] Guid pupilId, [FromRoute] Guid lessonId)
        {
            try
            {
                var deletePupilLesson = new DeletePupilLesson(pupilId, lessonId);

                var result = await mediator.Send(deletePupilLesson);

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