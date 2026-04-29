using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MMZM.BloodDonationMS.Domain.Features.BloodDonations;
using MMZM.BloodDonationMS.MVCV1.Services;
using System.Security.Claims;

namespace MMZM.BloodDonationMS.MVCV1.Controllers
{
    [Authorize]
    public class DonationController : Controller
    {
        private readonly ApiService _apiService;

        public DonationController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var response = await _apiService.GetAsync<GetDonationHistoryResponse>($"BloodDonation/history/{userId}");
            return View(response?.Data ?? new List<BloodDonationDto>());
        }

        [HttpPost]
        public async Task<IActionResult> Complete(int id)
        {
            var request = new CompleteDonationRequest { DonationId = id };
            var response = await _apiService.PostAsync<CompleteDonationRequest, CompleteDonationResponse>("BloodDonation/complete", request);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            var request = new CancelDonationRequest { DonationId = id };
            var response = await _apiService.PostAsync<CancelDonationRequest, CancelDonationResponse>("BloodDonation/cancel", request);
            return RedirectToAction("Index");
        }
    }
}
