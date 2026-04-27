using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MMZM.BloodDonationMS.Domain.Features.UserManagement;

namespace MMZM.BloodDonationMS.Api.Features.UserManagement;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _service;

    public UserController(UserService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetUsersRequest request)
    {
        var response = await _service.GetAllAsync(request);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var response = await _service.GetByIdAsync(new GetUserByIdRequest { UserId = id });
        return Ok(response);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateUserRequest request)
    {
        var response = await _service.CreateAsync(request);
        return Ok(response);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Edit(EditUserRequest request)
    {
        var response = await _service.EditAsync(request);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _service.DeleteAsync(new DeleteUserRequest { UserId = id });
        return Ok(response);
    }

    [HttpPost("toggle-status")]
    public async Task<IActionResult> ToggleStatus(ToggleUserStatusRequest request)
    {
        var response = await _service.ToggleStatusAsync(request);
        return Ok(response);
    }
}