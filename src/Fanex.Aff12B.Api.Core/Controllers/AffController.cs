namespace Fanex.Aff12B.Api.Core.Controllers
{
    using Attributes;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Services;
    using System.ComponentModel.DataAnnotations;

    [Route("api/[controller]/[action]")]
    [Authorize]
    [ApiController]
    [ServiceFilter(typeof(AffFilterAttribute))]
    public class AffController(IAffService affService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> AccountOverview([Required] int affId)
            => Ok(await affService.GetAccountOverviewForCurrentMonth(affId));

        [HttpGet]
        public async Task<IActionResult> NetWinLoss([Required] int affId)
            => Ok(await affService.GetNetWinLossForCurrentMonth(affId));

        [HttpGet]
        public async Task<IActionResult> StatementHistory([Required] int affId)
            => Ok(await affService.GetStatementHistoryForCurrentMonth(affId));

        [HttpGet]
        public async Task<IActionResult> ReferrersStatistics([Required] int affId)
            => Ok(await affService.GetReferrerStatisticsForCurrentMonth(affId));

        [HttpGet]
        public async Task<IActionResult> LandingPageStatistics([Required] int affId)
            => Ok(await affService.GetLandingPageStatisticsForCurrentMonth(affId));
    }
}