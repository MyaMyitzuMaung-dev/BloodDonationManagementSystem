using Microsoft.Extensions.DependencyInjection;
using MMZM.BloodDonationMS.Domain.Features.Auth;
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
            services.AddScoped<AuthFeature>();
        }
    }
}
