using System;
using System.Collections.Generic;
using MMZM.BloodDonationMS.Database.AppDbContextModels;

namespace MMZM.BloodDonationMS.Domain.Features.UserManagement
{
    public class GetUsersRequest
    {
        public int? RoleId { get; set; }
        public string? SearchTerm { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetUsersResponse
    {
        public List<UserDto> Users { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class UserDto
    {
        public int UserId { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? BloodGroup { get; set; }
        public string RoleName { get; set; } = "";
        public bool IsAvailable { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class GetUserByIdRequest
    {
        public int UserId { get; set; }
    }

    public class GetUserByIdResponse
    {
        public int UserId { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? BloodGroup { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; } = "";
        public bool IsAvailable { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateUserRequest
    {
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public int RoleId { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? BloodGroup { get; set; }
    }

    public class CreateUserResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = "";
    }

    public class EditUserRequest
    {
        public int UserId { get; set; }
        public string Name { get; set; } = "";
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? BloodGroup { get; set; }
        public int RoleId { get; set; }
    }

    public class EditUserResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = "";
    }

    public class DeleteUserRequest
    {
        public int UserId { get; set; }
    }

    public class DeleteUserResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = "";
    }

    public class ToggleUserStatusRequest
    {
        public int UserId { get; set; }
    }

    public class ToggleUserStatusResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = "";
    }
}