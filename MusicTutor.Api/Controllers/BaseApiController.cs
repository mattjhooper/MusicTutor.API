using Microsoft.AspNetCore.Mvc;

namespace MusicTutor.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        public BaseApiController()
        {
        }        
    }
}