using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MusicTutor.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        protected readonly IMediator mediator;
        public BaseApiController(IMediator mediator)
        {
            this.mediator = mediator;
        }        
    }
}