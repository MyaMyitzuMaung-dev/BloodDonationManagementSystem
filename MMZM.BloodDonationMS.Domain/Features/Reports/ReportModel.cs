using System;
using System.Collections.Generic;

namespace MMZM.BloodDonationMS.Domain.Features.Reports
{
    public class DashboardStatsResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = "";
        public int TotalDonors { get; set; }
        public int TotalRequests { get; set; }
        public int PendingRequests { get; set; }
        public int CompletedDonations { get; set; }
        public List<BloodGroupStat> BloodGroupStats { get; set; } = new();
    }

    public class BloodGroupStat
    {
        public string BloodGroup { get; set; } = "";
        public int Count { get; set; }
    }

    public class MonthlyReportResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = "";
        public List<MonthlyStat> MonthlyDonations { get; set; } = new();
    }

    public class MonthlyStat
    {
        public string Month { get; set; } = "";
        public int Count { get; set; }
    }
}
