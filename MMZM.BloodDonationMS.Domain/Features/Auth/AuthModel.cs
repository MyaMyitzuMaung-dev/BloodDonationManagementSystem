using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMZM.BloodDonationMS.Domain.Features.Auth
{
    public class RegisterRequest
    {
        public int RoleId { get; set; } // Donor / Requester
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? BloodGroup { get; set; }
    }
    public class RegisterResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class LoginResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public string? Token { get; set; }
        public int? UserId { get; set; }
        public string? Role { get; set; }
    }
}
