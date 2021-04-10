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
    }
}