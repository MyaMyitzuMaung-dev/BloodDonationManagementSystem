using Microsoft.EntityFrameworkCore;
using MMZM.BloodDonationMS.Database.AppDbContextModels;

namespace MMZM.BloodDonationMS.Domain.Features.BloodRequests;

public class BloodRequestFeature
{
    private readonly AppDbContext _context;

    public BloodRequestFeature(AppDbContext context)
    {
        _context = context;
    }

    // ✅ Approve Request
    public async Task<bool> ApproveRequestAsync(int requestId)
    {
        var request = await _context.BloodRequests.FindAsync(requestId);
        if (request == null || request.Status != "Pending") return false;

        request.Status = "Approved";
        await _context.SaveChangesAsync();
        return true;
    }

    // 🧑‍⚕️ Create Request
    public async Task<CreateBloodRequestResponse> CreateAsync(CreateBloodRequestRequest request, int userId)
    {
        var entity = new BloodRequest
        {
            RequesterId = userId,
            PatientName = request.PatientName,
            BloodGroup = request.BloodGroup,
            HospitalName = request.HospitalName,
            Location = request.Location,
            UnitsNeeded = request.UnitsNeeded,
            Urgency = request.Urgency,
            Status = "Pending",
            RequestDate = DateTime.Now,
            IsDeleted = false
        };

        _context.BloodRequests.Add(entity);
        await _context.SaveChangesAsync();

        return new CreateBloodRequestResponse
        {
            IsSuccess = true,
            Message = "Request created successfully"
        };
    }

    // 📋 Get All
    public async Task<GetBloodRequestsResponse> GetAllAsync()
    {
        var list = await _context.BloodRequests
            .Where(x => x.IsDeleted == false)
            .Select(x => new BloodRequestDto
            {
                RequestId = x.RequestId,
                PatientName = x.PatientName,
                BloodGroup = x.BloodGroup,
                HospitalName = x.HospitalName,
                Status = x.Status
            })
            .ToListAsync();

        return new GetBloodRequestsResponse
        {
            IsSuccess = true,
            Message = "Success",
            Data = list
        };
    }

    // 🤝 Accept Request (Donor)
    public async Task<AcceptRequestResponse> AcceptAsync(int requestId, int donorId)
    {
        var request = await _context.BloodRequests
            .FirstOrDefaultAsync(x => x.RequestId == requestId && x.IsDeleted == false);

        if (request == null)
        {
            return new AcceptRequestResponse
            {
                IsSuccess = false,
                Message = "Request not found"
            };
        }

        if (request.Status != "Pending")
        {
            return new AcceptRequestResponse
            {
                IsSuccess = false,
                Message = "Already processed"
            };
        }

        request.Status = "Accepted";

        var donation = new BloodDonation
        {
            RequestId = requestId,
            DonorId = donorId,
            DonationDate = DateTime.Now,
            Status = "Accepted",
            IsDeleted = false
        };

        _context.BloodDonations.Add(donation);

        await _context.SaveChangesAsync();

        return new AcceptRequestResponse
        {
            IsSuccess = true,
            Message = "Request accepted"
        };
    }

    // ✍️ Add Comment
    public async Task<AddCommentResponse> AddCommentAsync(AddCommentRequest request, int userId)
    {
        var entity = new RequestComment
        {
            RequestId = request.RequestId,
            UserId = userId,
            CommentText = request.CommentText,
            CreatedAt = DateTime.Now,
            IsDeleted = false
        };

        _context.RequestComments.Add(entity);
        await _context.SaveChangesAsync();

        return new AddCommentResponse
        {
            IsSuccess = true,
            Message = "Comment added"
        };
    }

    // 💬 Get Comments
    public async Task<GetCommentsResponse> GetCommentsAsync(int requestId)
    {
        var list = await _context.RequestComments
            .Where(x => x.RequestId == requestId && x.IsDeleted == false)
            .OrderBy(x => x.CreatedAt)
            .Select(x => new CommentDto
            {
                CommentId = x.CommentId,
                UserName = x.User.Name ?? "System",
                CommentText = x.CommentText ?? "",
                CreatedAt = x.CreatedAt
            })
            .ToListAsync();

        return new GetCommentsResponse
        {
            IsSuccess = true,
            Message = "Success",
            Data = list
        };
    }

    // 🗑️ Delete Comment (Soft Delete)
    public async Task<DeleteCommentResponse> DeleteCommentAsync(int commentId, int userId)
    {
        var comment = await _context.RequestComments.FindAsync(commentId);

        if (comment == null || comment.IsDeleted)
            return new DeleteCommentResponse { IsSuccess = false, Message = "Comment not found" };

        if (comment.UserId != userId)
            return new DeleteCommentResponse { IsSuccess = false, Message = "Unauthorized" };

        comment.IsDeleted = true;
        await _context.SaveChangesAsync();

        return new DeleteCommentResponse
        {
            IsSuccess = true,
            Message = "Comment deleted"
        };
    }

    // 📋 Get My Requests
    public async Task<GetBloodRequestsResponse> GetMyRequestsAsync(int userId)
    {
        var list = await _context.BloodRequests
            .Where(x => x.RequesterId == userId && x.IsDeleted == false)
            .Select(x => new BloodRequestDto
            {
                RequestId = x.RequestId,
                PatientName = x.PatientName,
                BloodGroup = x.BloodGroup,
                HospitalName = x.HospitalName,
                Status = x.Status
            })
            .OrderByDescending(x => x.RequestId)
            .ToListAsync();

        return new GetBloodRequestsResponse
        {
            IsSuccess = true,
            Message = "Success",
            Data = list
        };
    }
}