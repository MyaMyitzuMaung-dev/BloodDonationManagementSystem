using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MMZM.BloodDonationMS.Domain.Features.Auth;
using MMZM.BloodDonationMS.MVCV1.Services;
using System.Security.Claims;

namespace MMZM.BloodDonationMS.MVCV1.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApiService _apiService;

        public AccountController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var response = await _apiService.PostAsync<LoginRequest, LoginResponse>("Auth/login", request);

            if (response != null && response.IsSuccess)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, request.Email),
                    new Claim("UserId", (response.UserId ?? 0).ToString()),
                    new Claim(ClaimTypes.Role, response.Role ?? "User"),
                    new Claim("Token", response.Token ?? "")
                };

                var identity = new ClaimsIdentity(claims, "Cookies");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("Cookies", principal);

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", response?.Message ?? "Login failed");
            return View(request);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var response = await _apiService.PostAsync<RegisterRequest, RegisterResponse>("Auth/register", request);

            if (response != null && response.IsSuccess)
            {
                return RedirectToAction("Login");
            }

            ModelState.AddModelError("", response?.Message ?? "Registration failed");
            return View(request);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            return RedirectToAction("Index", "Home");
        }
    }
}
