using System;
using System.Collections.Generic;

namespace MMZM.BloodDonationMS.Database.AppDbContextModels;

public partial class RequestComment
{
    public int CommentId { get; set; }

    public int RequestId { get; set; }

    public int UserId { get; set; }

    public string? CommentText { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual BloodRequest Request { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
