using System;
using System.Linq;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MusicTutor.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        public const string Pupils = "Pupils";
        public const string Instruments = "Instruments";
        public const string Auth = "AuthManagement";

        protected readonly IMediator mediator;
        public BaseApiController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        protected Guid UserId
        {
            get
            {
                ClaimsPrincipal principal = User as ClaimsPrincipal;
                var userId = System.Guid.Parse(principal.Claims.FirstOrDefault(c => c.Type == "Id")?.Value);

                return userId;
            }
        }
    }
}