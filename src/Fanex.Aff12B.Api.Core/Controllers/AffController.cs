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
        public async Task<IActionResult> AccountOverviewAsync([Required] int affId)
            => Ok(await affService.GetAccountOverviewForCurrentMonthAsync(affId));

        [HttpGet]
        public async Task<IActionResult> NetWinLossAsync([Required] int affId)
            => Ok(await affService.GetNetWinLossForCurrentMonthAsync(affId));

        [HttpGet]
        public async Task<IActionResult> StatementHistoryAsync([Required] int affId)
            => Ok(await affService.GetStatementHistoryForCurrentMonthAsync(affId));

        [HttpGet]
        public async Task<IActionResult> ReferrersStatisticsAsync([Required] int affId)
            => Ok(await affService.GetReferrerStatisticsForCurrentMonthAsync(affId));

        [HttpGet]
        public async Task<IActionResult> LandingPageStatisticsAsync([Required] int affId)
            => Ok(await affService.GetLandingPageStatisticsForCurrentMonthAsync(affId));
    }
}