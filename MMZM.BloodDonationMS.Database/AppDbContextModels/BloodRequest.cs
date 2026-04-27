using System;
using System.Collections.Generic;

namespace MMZM.BloodDonationMS.Database.AppDbContextModels;

public partial class BloodRequest
{
    public int RequestId { get; set; }

    public int RequesterId { get; set; }

    public string? PatientName { get; set; }

    public string? BloodGroup { get; set; }

    public string? HospitalName { get; set; }

    public string? Location { get; set; }

    public int? UnitsNeeded { get; set; }

    public string? Urgency { get; set; }

    public string? Status { get; set; }

    public DateTime? RequestDate { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<BloodDonation> BloodDonations { get; set; } = new List<BloodDonation>();

    public virtual ICollection<RequestComment> RequestComments { get; set; } = new List<RequestComment>();

    public virtual User Requester { get; set; } = null!;
}
