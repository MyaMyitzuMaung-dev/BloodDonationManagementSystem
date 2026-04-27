// Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using MMZM.BloodDonationMS.Domain.Features;
using MMZM.BloodDonationMS.Domain.Features.Auth;

namespace MMZM.BloodDonationMS.Api.Features;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _feature;

    public AuthController(AuthService feature)
    {
        _feature = feature;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var response = await _feature.RegisterAsync(request);
        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var response = await _feature.LoginAsync(request);
        return Ok(response);
    }
}