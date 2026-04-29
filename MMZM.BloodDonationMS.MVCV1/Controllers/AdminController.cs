using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MMZM.BloodDonationMS.Domain.Features.Reports;
using MMZM.BloodDonationMS.Domain.Features.UserManagement;
using MMZM.BloodDonationMS.MVCV1.Services;

namespace MMZM.BloodDonationMS.MVCV1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApiService _apiService;

        public AdminController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Dashboard()
        {
            var response = await _apiService.GetAsync<DashboardStatsResponse>("Report/dashboard-stats");
            return View(response);
        }

        public async Task<IActionResult> PendingRequests()
        {
            var response = await _apiService.GetAsync<MMZM.BloodDonationMS.Domain.Features.BloodRequests.GetBloodRequestsResponse>("BloodRequest");
            var pending = response?.Data?.Where(x => x.Status == "Pending").ToList() 
                           ?? new List<MMZM.BloodDonationMS.Domain.Features.BloodRequests.BloodRequestDto>();
            return View(pending);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveRequest(int id)
        {
            var response = await _apiService.PostAsync<object, object>($"BloodRequest/approve/{id}", new { });
            return RedirectToAction("PendingRequests");
        }

        public async Task<IActionResult> Users(int pageNumber = 1, int? roleId = null, string searchTerm = "")
        {
            var endpoint = $"User?pageNumber={pageNumber}&pageSize=10";
            if (roleId.HasValue) endpoint += $"&roleId={roleId.Value}";
            if (!string.IsNullOrEmpty(searchTerm)) endpoint += $"&searchTerm={searchTerm}";

            var response = await _apiService.GetAsync<GetUsersResponse>(endpoint);
            ViewBag.RoleId = roleId;
            ViewBag.SearchTerm = searchTerm;
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleUserStatus(int id)
        {
            var request = new ToggleUserStatusRequest { UserId = id };
            await _apiService.PostAsync<ToggleUserStatusRequest, ToggleUserStatusResponse>("User/toggle-status", request);
            return RedirectToAction("Users");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _apiService.DeleteAsync($"User/{id}");
            return RedirectToAction("Users");
        }
    }
}
