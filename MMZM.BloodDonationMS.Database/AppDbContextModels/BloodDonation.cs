using System;
using System.Collections.Generic;

namespace MMZM.BloodDonationMS.Database.AppDbContextModels;

public partial class BloodDonation
{
    public int DonationId { get; set; }

    public int RequestId { get; set; }

    public int DonorId { get; set; }

    public DateTime? DonationDate { get; set; }

    public string? Status { get; set; }

    public bool IsDeleted { get; set; }

    public virtual User Donor { get; set; } = null!;

    public virtual BloodRequest Request { get; set; } = null!;
}
