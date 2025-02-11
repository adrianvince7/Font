using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Font.Core.Domain.Meeting;
using Font.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Font.Infrastructure.Data.Repositories
{
    public class MeetingRepository : IMeetingRepository
    {
        private readonly FontDbContext _context;

        public MeetingRepository(FontDbContext context)
        {
            _context = context;
        }

        public async Task<Meeting> GetByIdAsync(Guid id)
        {
            return await _context.Meetings
                .Include(m => m.Participants)
                .Include(m => m.RecurrencePattern)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Meeting>> GetAllAsync()
        {
            return await _context.Meetings
                .Include(m => m.Participants)
                .Include(m => m.RecurrencePattern)
                .ToListAsync();
        }

        public async Task<IEnumerable<Meeting>> GetUpcomingMeetingsAsync(DateTime fromDate)
        {
            return await _context.Meetings
                .Include(m => m.Participants)
                .Include(m => m.RecurrencePattern)
                .Where(m => m.StartTime >= fromDate)
                .OrderBy(m => m.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Meeting>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Meetings
                .Include(m => m.Participants)
                .Include(m => m.RecurrencePattern)
                .Where(m => m.Participants.Any(p => p.UserId == userId))
                .OrderBy(m => m.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Meeting>> GetNearbyMeetingsAsync(double latitude, double longitude, double radiusInKm)
        {
            // Using the Haversine formula to calculate distance
            const double earthRadiusKm = 6371.0;
            var meetings = await _context.Meetings
                .Include(m => m.Participants)
                .Include(m => m.RecurrencePattern)
                .Where(m => m.IsPrivate == false)
                .ToListAsync();

            return meetings.Where(m =>
            {
                var dLat = ToRad(m.Latitude - latitude);
                var dLon = ToRad(m.Longitude - longitude);
                var lat1 = ToRad(latitude);
                var lat2 = ToRad(m.Latitude);

                var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                        Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);
                var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                var distance = earthRadiusKm * c;

                return distance <= radiusInKm;
            });
        }

        private static double ToRad(double degrees)
        {
            return degrees * (Math.PI / 180);
        }

        public async Task<Meeting> AddAsync(Meeting meeting)
        {
            await _context.Meetings.AddAsync(meeting);
            await _context.SaveChangesAsync();
            return meeting;
        }

        public async Task UpdateAsync(Meeting meeting)
        {
            _context.Entry(meeting).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var meeting = await _context.Meetings.FindAsync(id);
            if (meeting != null)
            {
                _context.Meetings.Remove(meeting);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Meetings.AnyAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Meeting>> SearchAsync(string searchTerm, MeetingType? type = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = _context.Meetings
                .Include(m => m.Participants)
                .Include(m => m.RecurrencePattern)
                .Where(m => m.IsPrivate == false);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(m =>
                    m.Name.ToLower().Contains(searchTerm) ||
                    m.Description.ToLower().Contains(searchTerm) ||
                    m.Location.ToLower().Contains(searchTerm) ||
                    m.Tags.Any(t => t.ToLower().Contains(searchTerm)));
            }

            if (type.HasValue)
            {
                query = query.Where(m => m.Type == type.Value);
            }

            if (fromDate.HasValue)
            {
                query = query.Where(m => m.StartTime >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(m => m.StartTime <= toDate.Value);
            }

            return await query.OrderBy(m => m.StartTime).ToListAsync();
        }

        public async Task<IEnumerable<Meeting>> GetByTagsAsync(string[] tags)
        {
            if (tags == null || !tags.Any())
                return Array.Empty<Meeting>();

            var loweredTags = tags.Select(t => t.ToLower()).ToArray();
            return await _context.Meetings
                .Include(m => m.Participants)
                .Include(m => m.RecurrencePattern)
                .Where(m => m.Tags.Any(t => loweredTags.Contains(t.ToLower())))
                .OrderBy(m => m.StartTime)
                .ToListAsync();
        }

        public async Task AddParticipantAsync(Guid meetingId, MeetingParticipant participant)
        {
            var meeting = await GetByIdAsync(meetingId);
            if (meeting == null)
                throw new InvalidOperationException("Meeting not found");

            meeting.AddParticipant(participant);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveParticipantAsync(Guid meetingId, Guid participantId)
        {
            var meeting = await GetByIdAsync(meetingId);
            if (meeting == null)
                throw new InvalidOperationException("Meeting not found");

            meeting.RemoveParticipant(participantId);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateParticipantStatusAsync(Guid meetingId, Guid participantId, ParticipantStatus status)
        {
            var participant = await _context.MeetingParticipants
                .FirstOrDefaultAsync(p => p.MeetingId == meetingId && p.UserId == participantId);

            if (participant == null)
                throw new InvalidOperationException("Participant not found");

            participant.UpdateStatus(status);
            await _context.SaveChangesAsync();
        }
    }
} 