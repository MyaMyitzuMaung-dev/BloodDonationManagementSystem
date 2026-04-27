using Microsoft.EntityFrameworkCore;
using MMZM.BloodDonationMS.Database.AppDbContextModels;
using MMZM.BloodDonationMS.Domain.Features.UserManagement;

namespace MMZM.BloodDonationMS.Domain.Features.UserManagement;

public class UserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<GetUsersResponse> GetAllAsync(GetUsersRequest request)
    {
        var query = _context.Users
            .Include(x => x.Role)
            .Where(x => x.IsDeleted == false);

        if (request.RoleId.HasValue)
        {
            query = query.Where(x => x.RoleId == request.RoleId.Value);
        }

        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            query = query.Where(x => x.Name.Contains(request.SearchTerm) || x.Email.Contains(request.SearchTerm));
        }

        var totalCount = await query.CountAsync();

        var users = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new UserDto
            {
                UserId = x.UserId,
                Name = x.Name,
                Email = x.Email,
                Phone = x.Phone,
                Address = x.Address,
                BloodGroup = x.BloodGroup,
                RoleName = x.Role!.RoleName,
                IsAvailable = x.IsAvailable ?? false,
                CreatedAt = x.CreatedAt ?? DateTime.Now
            })
            .ToListAsync();

        return new GetUsersResponse
        {
            Users = users,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }

    public async Task<GetUserByIdResponse> GetByIdAsync(GetUserByIdRequest request)
    {
        var user = await _context.Users
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.UserId == request.UserId && x.IsDeleted == false);

        if (user == null)
        {
            return new GetUserByIdResponse
            {
                UserId = 0,
                Name = ""
            };
        }

        return new GetUserByIdResponse
        {
            UserId = user.UserId,
            Name = user.Name,
            Email = user.Email,
            Phone = user.Phone,
            Address = user.Address,
            BloodGroup = user.BloodGroup,
            RoleId = user.RoleId,
            RoleName = user.Role!.RoleName,
            IsAvailable = user.IsAvailable ?? false,
            CreatedAt = user.CreatedAt ?? DateTime.Now
        };
    }

    public async Task<CreateUserResponse> CreateAsync(CreateUserRequest request)
    {
        var exist = await _context.Users
            .FirstOrDefaultAsync(x => x.Email == request.Email && x.IsDeleted == false);

        if (exist != null)
        {
            return new CreateUserResponse
            {
                IsSuccess = false,
                Message = "Email already exists"
            };
        }

        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            RoleId = request.RoleId,
            Phone = request.Phone,
            Address = request.Address,
            BloodGroup = request.BloodGroup,
            IsAvailable = true,
            CreatedAt = DateTime.Now,
            IsDeleted = false
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new CreateUserResponse
        {
            IsSuccess = true,
            Message = "User created successfully"
        };
    }

    public async Task<EditUserResponse> EditAsync(EditUserRequest request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.UserId == request.UserId && x.IsDeleted == false);

        if (user == null)
        {
            return new EditUserResponse
            {
                IsSuccess = false,
                Message = "User not found"
            };
        }

        user.Name = request.Name;
        user.Phone = request.Phone;
        user.Address = request.Address;
        user.BloodGroup = request.BloodGroup;
        user.RoleId = request.RoleId;

        await _context.SaveChangesAsync();

        return new EditUserResponse
        {
            IsSuccess = true,
            Message = "User updated successfully"
        };
    }

    public async Task<DeleteUserResponse> DeleteAsync(DeleteUserRequest request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.UserId == request.UserId && x.IsDeleted == false);

        if (user == null)
        {
            return new DeleteUserResponse
            {
                IsSuccess = false,
                Message = "User not found"
            };
        }

        user.IsDeleted = true;
        await _context.SaveChangesAsync();

        return new DeleteUserResponse
        {
            IsSuccess = true,
            Message = "User deleted successfully"
        };
    }

    public async Task<ToggleUserStatusResponse> ToggleStatusAsync(ToggleUserStatusRequest request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.UserId == request.UserId && x.IsDeleted == false);

        if (user == null)
        {
            return new ToggleUserStatusResponse
            {
                IsSuccess = false,
                Message = "User not found"
            };
        }

        user.IsAvailable = !user.IsAvailable;
        await _context.SaveChangesAsync();

        var isAvailable = user.IsAvailable ?? false;
        return new ToggleUserStatusResponse
        {
            IsSuccess = true,
            Message = isAvailable ? "User activated" : "User deactivated"
        };
    }
}