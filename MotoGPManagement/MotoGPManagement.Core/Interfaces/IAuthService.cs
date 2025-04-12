using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MotoGPManagement.Application.DTOs;
using Microsoft.AspNetCore.Identity;

namespace MotoGPManagement.Core.Interfaces
{
    public interface IAuthService
    {
        // Authentication
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthResponseDto> RegisterAsync(RegisterUserDto registerDto);
        Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto);
        Task<bool> LogoutAsync(string userId);
        Task<bool> RevokeTokenAsync(string userId);

        // Password Management
        Task<bool> ChangePasswordAsync(ChangePasswordDto changePasswordDto);
        Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        Task<bool> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
        Task<bool> ValidateResetTokenAsync(string userId, string token);

        // User Management
        Task<UserDto> GetUserByIdAsync(string userId);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> UpdateUserProfileAsync(UpdateUserProfileDto updateDto);
        Task<bool> DeleteUserAsync(string userId);
        Task<bool> LockUserAsync(string userId, TimeSpan duration);
        Task<bool> UnlockUserAsync(string userId);

        // Role Management
        Task<IEnumerable<string>> GetUserRolesAsync(string userId);
        Task<bool> AssignRoleAsync(string userId, string role);
        Task<bool> RemoveRoleAsync(string userId, string role);
        Task<bool> IsInRoleAsync(string userId, string role);

        // Email Verification
        Task<bool> SendVerificationEmailAsync(string userId);
        Task<bool> VerifyEmailAsync(string userId, string token);
        Task<bool> ResendVerificationEmailAsync(string email);

        // Two-Factor Authentication
        Task<bool> EnableTwoFactorAsync(string userId);
        Task<bool> DisableTwoFactorAsync(string userId);
        Task<bool> GenerateTwoFactorTokenAsync(string userId);
        Task<bool> ValidateTwoFactorTokenAsync(string userId, string token);

        // Security
        Task<IEnumerable<UserLoginHistoryDto>> GetUserLoginHistoryAsync(string userId);
        Task<bool> AddLoginHistoryAsync(UserLoginHistoryDto historyDto);
        Task<bool> IsEmailUniqueAsync(string email);
        Task<bool> ValidatePasswordStrengthAsync(string password);

        // Team Manager Management
        Task<TeamManagerDto> AssignTeamManagerRoleAsync(CreateTeamManagerDto managerDto);
        Task<TeamManagerDto> GetTeamManagerByUserIdAsync(string userId);
        Task<bool> RemoveTeamManagerRoleAsync(string userId);
        Task<IEnumerable<TeamManagerDto>> GetAllTeamManagersAsync();

        // Session Management
        Task<bool> ValidateSessionAsync(string userId, string sessionId);
        Task<bool> InvalidateAllSessionsAsync(string userId);
        Task<Dictionary<string, DateTime>> GetActiveSessionsAsync(string userId);
    }
}
