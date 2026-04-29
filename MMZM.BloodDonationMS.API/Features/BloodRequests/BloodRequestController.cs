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

    // ✅ Approve (Admin Only)
    [Authorize(Roles = "Admin")]
    [HttpPost("approve/{id}")]
    public async Task<IActionResult> Approve(int id)
    {
        var success = await _feature.ApproveRequestAsync(id);
        if (!success) return BadRequest("Could not approve request");
        return Ok(new { IsSuccess = true, Message = "Request Approved" });
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

    // 📋 Get My Requests
    [HttpGet("my-requests")]
    public async Task<IActionResult> GetMyRequests()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var response = await _feature.GetMyRequestsAsync(userId);
        return Ok(response);
    }

    // ✍️ Add Comment
    [HttpPost("comment")]
    public async Task<IActionResult> AddComment(AddCommentRequest request)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var response = await _feature.AddCommentAsync(request, userId);
        return Ok(response);
    }

    // 💬 Get Comments
    [HttpGet("{requestId}/comments")]
    public async Task<IActionResult> GetComments(int requestId)
    {
        var response = await _feature.GetCommentsAsync(requestId);
        return Ok(response);
    }

    // 🗑️ Delete Comment
    [HttpDelete("comment/{commentId}")]
    public async Task<IActionResult> DeleteComment(int commentId)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var response = await _feature.DeleteCommentAsync(commentId, userId);
        return Ok(response);
    }
}