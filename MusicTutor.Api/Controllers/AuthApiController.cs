using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace MusicTutor.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public abstract class AuthApiController : BaseApiController
    {
        public AuthApiController(IMediator mediator) : base(mediator)
        {
        }
    }
}