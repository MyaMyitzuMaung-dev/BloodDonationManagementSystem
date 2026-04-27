using Microsoft.Extensions.DependencyInjection;
using MMZM.BloodDonationMS.Domain.Features;
using MMZM.BloodDonationMS.Domain.Features.BloodDonations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMZM.BloodDonationMS.Domain
{
    public static class FeatureManager
    {
        public static void AddFeatures(this IServiceCollection services)
        {
            services.AddScoped<AuthService>();
            services.AddScoped<BloodDonationService>();
        }
    }
}
