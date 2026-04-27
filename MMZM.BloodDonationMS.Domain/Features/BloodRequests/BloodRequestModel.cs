namespace MMZM.BloodDonationMS.Domain.Features.BloodRequests;

public class CreateBloodRequestRequest
{
    public string PatientName { get; set; }
    public string BloodGroup { get; set; }
    public string HospitalName { get; set; }
    public string Location { get; set; }
    public int UnitsNeeded { get; set; }
    public string Urgency { get; set; }
}
public class CreateBloodRequestResponse
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
}
public class GetBloodRequestsResponse
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }

    public List<BloodRequestDto> Data { get; set; }
}

public class BloodRequestDto
{
    public int RequestId { get; set; }
    public string PatientName { get; set; }
    public string BloodGroup { get; set; }
    public string HospitalName { get; set; }
    public string Status { get; set; }
}
public class AcceptRequestRequest
{
    public int RequestId { get; set; }
}
public class AcceptRequestResponse
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
}