using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MMZM.BloodDonationMS.Domain.Features.Reports;

namespace MMZM.BloodDonationMS.Api.Features
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly ReportService _feature;

        public ReportController(ReportService feature)
        {
            _feature = feature;
        }

        [HttpGet("dashboard-stats")]
        public async Task<IActionResult> GetDashboardStats()
        {
            var response = await _feature.GetDashboardStatsAsync();
            return Ok(response);
        }

        [HttpGet("monthly-report")]
        public async Task<IActionResult> GetMonthlyReport()
        {
            var response = await _feature.GetMonthlyReportAsync();
            return Ok(response);
        }
    }
}
