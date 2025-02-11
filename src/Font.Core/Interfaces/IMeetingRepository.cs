using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Font.Core.Domain.Meeting;

namespace Font.Core.Interfaces
{
    public interface IMeetingRepository
    {
        Task<Meeting> GetByIdAsync(Guid id);
        Task<IEnumerable<Meeting>> GetAllAsync();
        Task<IEnumerable<Meeting>> GetUpcomingMeetingsAsync(DateTime fromDate);
        Task<IEnumerable<Meeting>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<Meeting>> GetNearbyMeetingsAsync(double latitude, double longitude, double radiusInKm);
        Task<Meeting> AddAsync(Meeting meeting);
        Task UpdateAsync(Meeting meeting);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<IEnumerable<Meeting>> SearchAsync(string searchTerm, MeetingType? type = null, DateTime? fromDate = null, DateTime? toDate = null);
        Task<IEnumerable<Meeting>> GetByTagsAsync(string[] tags);
        Task AddParticipantAsync(Guid meetingId, MeetingParticipant participant);
        Task RemoveParticipantAsync(Guid meetingId, Guid participantId);
        Task UpdateParticipantStatusAsync(Guid meetingId, Guid participantId, ParticipantStatus status);
    }
} 