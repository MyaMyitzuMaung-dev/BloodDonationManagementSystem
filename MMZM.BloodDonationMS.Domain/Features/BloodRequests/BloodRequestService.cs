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
}