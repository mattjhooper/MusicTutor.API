using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Http;
using MusicTutor.Api.Contracts.Errors;
using MusicTutor.Api.Queries.Pupils;
using MusicTutor.Api.Commands.Pupils;
using System.Collections.Generic;
using MusicTutor.Api.Contracts.Payments;

namespace MusicTutor.Api.Controllers.Pupils
{
    [Route(Pupils)]
    public class PupilPaymentsController : AuthApiController
    {
        public PupilPaymentsController(IMediator mediator) : base(mediator)
        {
        }


        /// <summary>
        /// Gets the PupilPayment with the given id
        /// </summary>
        /// <param name="pupilId"></param>
        /// <param name="paymentId"></param>
        /// <returns></returns>
        [HttpGet("{pupilId}/Payments/{paymentId}", Name = "GetSinglePupilPayment")]
        [ProducesResponseType(typeof(PaymentResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PaymentResponseDto>> GetSingleAsync([FromRoute] Guid pupilId, [FromRoute] Guid paymentId)
        {
            var req = new WithMusicTutorUserId<GetPupilPaymentById, PaymentResponseDto>(UserId, new GetPupilPaymentById(pupilId, paymentId));
            var payment = await mediator.Send(req);

            if (payment is null)
                return NotFound();

            return Ok(payment);
        }

        /// <summary>
        /// Gets all Pupil Payments
        /// </summary>
        /// <returns></returns>
        [HttpGet("{pupilId}/Payments", Name = "GetPupilPayments")]
        [ProducesResponseType(typeof(IEnumerable<PaymentResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<PaymentResponseDto>>> GetManyAsync([FromRoute] Guid pupilId)
        {
            var req = new WithMusicTutorUserId<GetPupilPayments, IEnumerable<PaymentResponseDto>>(UserId, new GetPupilPayments(pupilId));
            var payments = await mediator.Send(req);

            return Ok(payments);
        }

        /// <summary>
        /// Creates a new Pupil Payment and returns the Payment
        /// </summary>
        /// <param name="pupilId"></param>
        /// <param name="item"></param>
        /// <returns>If successful it returns a CreatedAtRoute response - see
        /// https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-2.1#implement-the-other-crud-operations
        /// </returns>
        [ProducesResponseType(typeof(PaymentResponseDto), StatusCodes.Status201Created)] //You need this, otherwise Swagger says the success status is 200, not 201
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [HttpPost("{pupilId}/Payments", Name = "AddPupilPayment")]
        public async Task<ActionResult<PaymentResponseDto>> PostAsync([FromRoute] Guid pupilId, [FromBody] CreatePupilPayment item)
        {
            if (pupilId != item.PupilId)
                return BadRequest($"Route Id [{pupilId}] must match message Body Id [{item.PupilId}].");

            try
            {
                var req = new WithMusicTutorUserId<CreatePupilPayment, PaymentResponseDto>(UserId, item);
                var result = await mediator.Send(req);

                if (result is null)
                    return NotFound();

                //NOTE: to get this to work you MUST set the name of the HttpGet, e.g. [HttpGet("{id}", Name= "GetSinglePayment")],
                //on the Get you want to call, then then use the Name value in the Response.
                //Otherwise you get a "No route matches the supplied values" error.
                //see https://stackoverflow.com/questions/36560239/asp-net-core-createdatroute-failure for more on this
                return CreatedAtRoute("GetSinglePupilPayment", new { pupilId = item.PupilId, paymentId = result.Id }, result);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
        }

        /// <summary>
        /// Deletes the Pupil Payment
        /// </summary>
        /// <param name="pupilId"></param>
        /// <param name="paymentId"></param>
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [HttpDelete("{pupilId}/payments/{paymentId}", Name = "DeletePupilPayment")]
        public async Task<ActionResult> DeleteAsync([FromRoute] Guid pupilId, [FromRoute] Guid paymentId)
        {
            try
            {
                var deletePupilPayment = new DeletePupilPayment(pupilId, paymentId);
                var req = new WithMusicTutorUserId<DeletePupilPayment, int>(UserId, deletePupilPayment);

                var result = await mediator.Send(req);

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