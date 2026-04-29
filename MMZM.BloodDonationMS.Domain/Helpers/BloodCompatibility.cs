using System.Collections.Generic;

namespace MMZM.BloodDonationMS.Domain.Helpers
{
    public static class BloodCompatibility
    {
        private static readonly Dictionary<string, List<string>> _canDonateTo = new()
        {
            { "O-", new() { "O-", "O+", "A-", "A+", "B-", "B+", "AB-", "AB+" } },
            { "O+", new() { "O+", "A+", "B+", "AB+" } },
            { "A-", new() { "A-", "A+", "AB-", "AB+" } },
            { "A+", new() { "A+", "AB+" } },
            { "B-", new() { "B-", "B+", "AB-", "AB+" } },
            { "B+", new() { "B+", "AB+" } },
            { "AB-", new() { "AB-", "AB+" } },
            { "AB+", new() { "AB+" } }
        };

        private static readonly Dictionary<string, List<string>> _canReceiveFrom = new()
        {
            { "O-", new() { "O-" } },
            { "O+", new() { "O-", "O+" } },
            { "A-", new() { "O-", "A-" } },
            { "A+", new() { "O-", "O+", "A-", "A+" } },
            { "B-", new() { "O-", "B-" } },
            { "B+", new() { "O-", "O+", "B-", "B+" } },
            { "AB-", new() { "O-", "A-", "B-", "AB-" } },
            { "AB+", new() { "O-", "O+", "A-", "A+", "B-", "B+", "AB-", "AB+" } }
        };

        public static List<string> GetCompatibleRecipients(string donorBloodGroup)
        {
            if (_canDonateTo.TryGetValue(donorBloodGroup, out var compatible))
                return compatible;
            return new List<string> { donorBloodGroup };
        }

        public static List<string> GetCompatibleDonors(string recipientBloodGroup)
        {
            if (_canReceiveFrom.TryGetValue(recipientBloodGroup, out var compatible))
                return compatible;
            return new List<string> { recipientBloodGroup };
        }

        public static bool IsCompatible(string donorGroup, string recipientGroup)
        {
            return _canDonateTo.ContainsKey(donorGroup) && _canDonateTo[donorGroup].Contains(recipientGroup);
        }
    }
}
