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

        public async Task<IActionResult> Matching()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            // 1. Get Donor Info
            var donor = await _apiService.GetAsync<MMZM.BloodDonationMS.Domain.Features.UserManagement.GetUserByIdResponse>($"User/{userId}");
            if (donor == null || donor.BloodGroup == null) return View(new List<MMZM.BloodDonationMS.Domain.Features.BloodRequests.BloodRequestDto>());

            // 2. Check Compatibility
            var compatibleGroups = MMZM.BloodDonationMS.Domain.Helpers.BloodCompatibility.GetCompatibleRecipients(donor.BloodGroup);
            
            // 3. Get All Requests and Filter
            var requestsResponse = await _apiService.GetAsync<MMZM.BloodDonationMS.Domain.Features.BloodRequests.GetBloodRequestsResponse>("BloodRequest");
            var matching = requestsResponse?.Data?.Where(r => r.Status == "Pending" && compatibleGroups.Contains(r.BloodGroup)).ToList() 
                           ?? new List<MMZM.BloodDonationMS.Domain.Features.BloodRequests.BloodRequestDto>();

            ViewBag.DonorGroup = donor.BloodGroup;
            ViewBag.IsAvailable = donor.IsAvailable;
            
            return View(matching);
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
