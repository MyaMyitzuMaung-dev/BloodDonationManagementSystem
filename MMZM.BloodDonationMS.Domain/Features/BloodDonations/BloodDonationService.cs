using Microsoft.EntityFrameworkCore;
using MMZM.BloodDonationMS.Database.AppDbContextModels;

namespace MMZM.BloodDonationMS.Domain.Features.BloodDonations;

public class BloodDonationFeature
{
    private readonly AppDbContext _context;

    public BloodDonationFeature(AppDbContext context)
    {
        _context = context;
    }

    // 📋 Get Donation History for Donor
    public async Task<GetDonationHistoryResponse> GetHistoryAsync(int donorId)
    {
        var list = await _context.BloodDonations
            .Where(x => x.DonorId == donorId && x.IsDeleted == false)
            .Select(x => new BloodDonationDto
            {
                DonationId = x.DonationId,
                RequestId = x.RequestId,
                PatientName = x.Request.PatientName ?? "Unknown",
                DonationDate = x.DonationDate,
                Status = x.Status ?? "Pending"
            })
            .OrderByDescending(x => x.DonationDate)
            .ToListAsync();

        return new GetDonationHistoryResponse
        {
            IsSuccess = true,
            Message = "Success",
            Data = list
        };
    }

    // ✅ Complete Donation
    public async Task<CompleteDonationResponse> CompleteAsync(CompleteDonationRequest request, int userId)
    {
        var donation = await _context.BloodDonations
            .Include(x => x.Request)
            .FirstOrDefaultAsync(x => x.DonationId == request.DonationId && x.IsDeleted == false);

        if (donation == null)
            return new CompleteDonationResponse { IsSuccess = false, Message = "Donation not found" };

        if (donation.DonorId != userId)
            return new CompleteDonationResponse { IsSuccess = false, Message = "Unauthorized" };

        if (donation.Status == "Completed")
            return new CompleteDonationResponse { IsSuccess = false, Message = "Already completed" };

        donation.Status = "Completed";
        donation.Request.Status = "Completed";

        // Update User's Last Donation Date
        var user = await _context.Users.FindAsync(userId);
        if (user != null)
        {
            user.LastDonationDate = DateOnly.FromDateTime(DateTime.Now);
        }

        await _context.SaveChangesAsync();

        return new CompleteDonationResponse
        {
            IsSuccess = true,
            Message = "Donation marked as completed"
        };
    }

    // ❌ Cancel Donation
    public async Task<CancelDonationResponse> CancelAsync(CancelDonationRequest request, int userId)
    {
        var donation = await _context.BloodDonations
            .Include(x => x.Request)
            .FirstOrDefaultAsync(x => x.DonationId == request.DonationId && x.IsDeleted == false);

        if (donation == null)
            return new CancelDonationResponse { IsSuccess = false, Message = "Donation not found" };

        // Donor can cancel, or Requester of the original request can cancel
        if (donation.DonorId != userId && donation.Request.RequesterId != userId)
            return new CancelDonationResponse { IsSuccess = false, Message = "Unauthorized" };

        donation.Status = "Cancelled";
        donation.Request.Status = "Pending"; // Reset request to pending so someone else can accept

        await _context.SaveChangesAsync();

        return new CancelDonationResponse
        {
            IsSuccess = true,
            Message = "Donation cancelled"
        };
    }
}
