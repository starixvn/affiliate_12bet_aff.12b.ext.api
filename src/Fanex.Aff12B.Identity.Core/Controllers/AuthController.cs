using Fanex.Aff12B.Identity.Core.Models.MetaModel;
using Fanex.Aff12B.Identity.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Aff12B.Identity.Core.Controllers
{
    [ApiController]
    public class AuthController(IAuthenticateService authService) : ControllerBase
    {
        [Route("api/Auth")]
        [HttpPost]
        public IActionResult RequestToken([FromBody] TokenRequestParam request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Request");
            }
            var result = authService.IsAuthenticated(request);

            return result.Item1 ? Ok(result.Item2)
                : BadRequest("Wrong username or password");
        }
    }
}