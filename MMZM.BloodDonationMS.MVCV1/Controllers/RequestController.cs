using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MMZM.BloodDonationMS.Domain.Features.BloodRequests;
using MMZM.BloodDonationMS.MVCV1.Services;

namespace MMZM.BloodDonationMS.MVCV1.Controllers
{
    [Authorize]
    public class RequestController : Controller
    {
        private readonly ApiService _apiService;

        public RequestController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var response = await _apiService.GetAsync<GetBloodRequestsResponse>("BloodRequest");
            return View(response?.Data ?? new List<BloodRequestDto>());
        }

        public async Task<IActionResult> Details(int id)
        {
            var requestsResponse = await _apiService.GetAsync<GetBloodRequestsResponse>("BloodRequest");
            var request = requestsResponse?.Data?.FirstOrDefault(x => x.RequestId == id);
            
            if (request == null) return NotFound();

            var commentsResponse = await _apiService.GetAsync<GetCommentsResponse>($"BloodRequest/comments/{id}");
            ViewBag.Comments = commentsResponse?.Data ?? new List<CommentDto>();

            return View(request);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBloodRequestRequest request)
        {
            var response = await _apiService.PostAsync<CreateBloodRequestRequest, CreateBloodRequestResponse>("BloodRequest", request);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", response?.Message ?? "Failed to create request");
            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> Accept(int id)
        {
            var request = new AcceptRequestRequest { RequestId = id };
            var response = await _apiService.PostAsync<AcceptRequestRequest, AcceptRequestResponse>("BloodRequest/accept", request);
            return RedirectToAction("Index", "Donation");
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(AddCommentRequest request)
        {
            var response = await _apiService.PostAsync<AddCommentRequest, AddCommentResponse>("BloodRequest/comment", request);
            return RedirectToAction("Details", new { id = request.RequestId });
        }
    }
}
