using Font.Application.Features.Meetings.Queries.GetMeeting;

namespace Font.Application.Common.Interfaces
{
    public interface IMeetingService
    {
        Task<(bool success, string meetingId)> CreateMeetingAsync(
            string title,
            string description,
            DateTime startTime,
            DateTime endTime,
            string hostId,
            List<string> participantIds,
            bool isPrivate,
            Dictionary<string, string> settings);

        Task<MeetingDto?> GetMeetingAsync(string meetingId);
        Task<bool> JoinMeetingAsync(string meetingId, string userId);
        Task<bool> LeaveMeetingAsync(string meetingId, string userId);
        Task<bool> UpdateMeetingAsync(string meetingId, string title, string description, DateTime startTime, DateTime endTime);
        Task<bool> DeleteMeetingAsync(string meetingId);
        Task<List<MeetingDto>> SearchMeetingsAsync(string searchTerm, int page, int pageSize);
    }
} 