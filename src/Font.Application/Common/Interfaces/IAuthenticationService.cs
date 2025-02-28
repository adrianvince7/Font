namespace Font.Application.Common.Interfaces
{
    public interface IAuthenticationService
    {
        Task<(bool success, string message)> RegisterAsync(string username, string email, string password);
        Task<(bool success, string token)> LoginAsync(string email, string password);
        Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task<bool> ResetPasswordAsync(string email);
        Task<bool> ValidateTokenAsync(string token);
    }
} 