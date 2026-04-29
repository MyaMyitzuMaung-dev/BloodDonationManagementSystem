using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MMZM.BloodDonationMS.Domain.Features.BloodDonations;
using System.Security.Claims;

namespace MMZM.BloodDonationMS.Api.Features;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BloodDonationController : ControllerBase
{
    private readonly BloodDonationFeature _feature;

    public BloodDonationController(BloodDonationFeature feature)
    {
        _feature = feature;
    }

    // 📋 History
    [HttpGet("history")]
    public async Task<IActionResult> GetHistory()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var response = await _feature.GetHistoryAsync(userId);
        return Ok(response);
    }

    // ✅ Complete
    [HttpPost("complete")]
    public async Task<IActionResult> Complete(CompleteDonationRequest request)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var response = await _feature.CompleteAsync(request, userId);
        return Ok(response);
    }

    // ❌ Cancel
    [HttpPost("cancel")]
    public async Task<IActionResult> Cancel(CancelDonationRequest request)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var response = await _feature.CancelAsync(request, userId);
        return Ok(response);
    }
}
