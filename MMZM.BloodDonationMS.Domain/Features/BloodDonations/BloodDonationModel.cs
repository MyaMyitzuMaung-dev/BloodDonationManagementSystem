namespace MMZM.BloodDonationMS.Domain.Features.BloodDonations;

public class GetDonationHistoryResponse
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = "";
    public List<BloodDonationDto> Data { get; set; } = new();
}

public class BloodDonationDto
{
    public int DonationId { get; set; }
    public int RequestId { get; set; }
    public string PatientName { get; set; } = "";
    public DateTime? DonationDate { get; set; }
    public string Status { get; set; } = "";
}

public class CompleteDonationRequest
{
    public int DonationId { get; set; }
}

public class CompleteDonationResponse
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = "";
}

public class CancelDonationRequest
{
    public int DonationId { get; set; }
}

public class CancelDonationResponse
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = "";
}
