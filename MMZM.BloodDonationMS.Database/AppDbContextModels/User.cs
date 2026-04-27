using System;
using System.Collections.Generic;

namespace MMZM.BloodDonationMS.Database.AppDbContextModels;

public partial class User
{
    public int UserId { get; set; }

    public int RoleId { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? PasswordHash { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? BloodGroup { get; set; }

    public DateOnly? LastDonationDate { get; set; }

    public bool? IsAvailable { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<BloodDonation> BloodDonations { get; set; } = new List<BloodDonation>();

    public virtual ICollection<BloodRequest> BloodRequests { get; set; } = new List<BloodRequest>();

    public virtual ICollection<RequestComment> RequestComments { get; set; } = new List<RequestComment>();

    public virtual Role Role { get; set; } = null!;
}
