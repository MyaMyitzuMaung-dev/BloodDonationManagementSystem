using Microsoft.EntityFrameworkCore;
using MMZM.BloodDonationMS.Database.AppDbContextModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMZM.BloodDonationMS.Domain.Features.Reports
{
    public class ReportService
    {
        private readonly AppDbContext _context;

        public ReportService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardStatsResponse> GetDashboardStatsAsync()
        {
            var totalDonors = await _context.Users.CountAsync(x => x.RoleId == 2 && !x.IsDeleted);
            var totalRequests = await _context.BloodRequests.CountAsync(x => !x.IsDeleted);
            var pendingRequests = await _context.BloodRequests.CountAsync(x => x.Status == "Pending" && !x.IsDeleted);
            var completedDonations = await _context.BloodDonations.CountAsync(x => x.Status == "Completed" && !x.IsDeleted);

            var bloodGroupStats = await _context.Users
                .Where(x => x.RoleId == 2 && !x.IsDeleted && x.BloodGroup != null)
                .GroupBy(x => x.BloodGroup)
                .Select(g => new BloodGroupStat
                {
                    BloodGroup = g.Key!,
                    Count = g.Count()
                })
                .ToListAsync();

            return new DashboardStatsResponse
            {
                IsSuccess = true,
                TotalDonors = totalDonors,
                TotalRequests = totalRequests,
                PendingRequests = pendingRequests,
                CompletedDonations = completedDonations,
                BloodGroupStats = bloodGroupStats
            };
        }

        public async Task<MonthlyReportResponse> GetMonthlyReportAsync()
        {
            var sixMonthsAgo = DateTime.Now.AddMonths(-6);
            
            var donations = await _context.BloodDonations
                .Where(x => x.DonationDate >= sixMonthsAgo && x.Status == "Completed" && !x.IsDeleted)
                .ToListAsync();

            var monthlyStats = donations
                .GroupBy(x => x.DonationDate.Value.ToString("MMM yyyy"))
                .Select(g => new MonthlyStat
                {
                    Month = g.Key,
                    Count = g.Count()
                })
                .OrderBy(x => DateTime.Parse(x.Month))
                .ToList();

            return new MonthlyReportResponse
            {
                IsSuccess = true,
                MonthlyDonations = monthlyStats
            };
        }
    }
}
