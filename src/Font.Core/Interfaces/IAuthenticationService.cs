using System;
using System.Threading.Tasks;
using Font.Core.Domain.User;

namespace Font.Core.Interfaces
{
    public interface IAuthenticationService
    {
        Task<(bool success, string token, User user)> AuthenticateAsync(string email, string password);
        Task<(bool success, string message)> RegisterAsync(string username, string email, string password);
        Task<bool> ValidateTokenAsync(string token);
        Task<(bool success, string message)> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);
        Task<(bool success, string message)> RequestPasswordResetAsync(string email);
        Task<(bool success, string message)> ResetPasswordAsync(string email, string token, string newPassword);
        Task<(bool success, string message)> ConfirmEmailAsync(string email, string token);
        Task<bool> RevokeTokenAsync(string token);
        Task<(bool success, string message)> InitiateExternalAuthAsync(string provider);
        Task<(bool success, string token, User user, string message)> CompleteExternalAuthAsync(string provider, string code);
    }
} 