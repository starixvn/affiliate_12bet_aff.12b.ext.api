using Fanex.Aff12B.Ext.Api.Core.Attributes;
using Fanex.Aff12B.Ext.Api.Core.Models.MetaModel;
using Fanex.Aff12B.Ext.Api.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Aff12B.Ext.Api.Core.Controllers
{
    [ApiController]
    [ServiceFilter(typeof(IPFilterAttribute))]
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