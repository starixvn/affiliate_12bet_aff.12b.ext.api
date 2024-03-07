using Fanex.Aff12B.Ext.Api.Core.Attributes;
using Fanex.Aff12B.Ext.Api.Core.Models.DTO;
using Fanex.Aff12B.Ext.Api.Core.Models.MetaModel;
using Fanex.Aff12B.Ext.Api.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Fanex.Aff12B.Ext.Api.Core.Controllers
{
    [Authorize]
    [ApiController]
    [ServiceFilter(typeof(IPFilterAttribute))]
    public class AffController : ControllerBase
    {
        private readonly ILogger<AffController> _logger;
        private readonly IAffService _affService;

        public AffController(
            ILogger<AffController> logger,
            IAffService affService)
        {
            _logger = logger;
            _affService = affService;
        }

        [HttpGet]
        [Route("api/StatementHistory")]
        public IEnumerable<StatementHistory> GetStatementHistory([FromQuery] StatementHistoryParam param)
        {
            _logger.LogInformation("Ext 12B - Get statement history");

            return _affService.GetStatementHistory(param);
        }
    }
}
