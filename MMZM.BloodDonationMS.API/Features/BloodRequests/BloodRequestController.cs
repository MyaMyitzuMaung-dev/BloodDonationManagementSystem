// BloodRequestController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MMZM.BloodDonationMS.Domain.Features.BloodRequests;
using System.Security.Claims;

namespace MMZM.BloodDonationMS.Api.Features;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BloodRequestController : ControllerBase
{
    private readonly BloodRequestFeature _feature;

    public BloodRequestController(BloodRequestFeature feature)
    {
        _feature = feature;
    }

    // 🧑‍⚕️ Create
    [Authorize(Roles = "Requester")]
    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateBloodRequestRequest request)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var response = await _feature.CreateAsync(request, userId);
        return Ok(response);
    }

    // 📋 Get All
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = await _feature.GetAllAsync();
        return Ok(response);
    }

    // 🤝 Accept
    [Authorize(Roles = "Donor")]
    [HttpPost("accept")]
    public async Task<IActionResult> Accept(AcceptRequestRequest request)
    {
        var donorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var response = await _feature.AcceptAsync(request.RequestId, donorId);
        return Ok(response);
    }
}