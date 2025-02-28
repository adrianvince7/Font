using Font.Application.Features.Users.Queries.GetProfile;

namespace Font.Application.Common.Interfaces
{
    public interface IUserService
    {
        Task<UserProfileDto?> GetProfileAsync(string userId);
        Task<bool> UpdateProfileAsync(string userId, string? displayName, string? bio, string? avatarUrl, Dictionary<string, string> preferences);
        Task<Dictionary<string, string>> GetPreferencesAsync(string userId);
        Task<bool> UpdatePreferenceAsync(string userId, string key, string value);
        Task<bool> DeletePreferenceAsync(string userId, string key);
    }
} 