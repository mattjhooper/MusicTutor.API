using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MusicTutor.Api.Commands.Auth;
using MusicTutor.Api.Contracts.Auth;

// https://dev.to/moe23/asp-net-core-5-rest-api-authentication-with-jwt-step-by-step-140d
namespace MusicTutor.Api.Controllers.Auth
{
    public class AuthManagementController : BaseApiController
    {
        public AuthManagementController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterUser user)
        {
            // Check if the incoming request is valid
            if (ModelState.IsValid)
            {
                // check if the user with the same email already exists
                var registrationResponse = await mediator.Send(user);

                if (!registrationResponse.Result)
                {
                    return BadRequest(registrationResponse);
                }

                return Ok(registrationResponse);
            }

            return BadRequest(InvalidModelResponse());
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUser user)
        {
            if (ModelState.IsValid)
            {
                // check if the user with the same email already exists
                var registrationResponse = await mediator.Send(user);

                if (!registrationResponse.Result)
                {
                    return BadRequest(registrationResponse);
                }

                return Ok(registrationResponse);
            }

            return BadRequest(InvalidModelResponse());
        }

        private RegistrationResponseDto InvalidModelResponse()
        {
            return new RegistrationResponseDto()
            {
                Result = false,
                Errors = new List<string>(){
                                        "Invalid payload"
                                    }
            };
        }
    }
}