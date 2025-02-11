using System;

namespace Font.Core.Domain.Meeting
{
    public class MeetingParticipant
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public Guid MeetingId { get; private set; }
        public DateTime JoinedAt { get; private set; }
        public ParticipantRole Role { get; private set; }
        public ParticipantStatus Status { get; private set; }

        private MeetingParticipant() { } // For EF Core

        public static MeetingParticipant Create(Guid userId, Guid meetingId, ParticipantRole role = ParticipantRole.Attendee)
        {
            return new MeetingParticipant
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                MeetingId = meetingId,
                JoinedAt = DateTime.UtcNow,
                Role = role,
                Status = ParticipantStatus.Registered
            };
        }

        public void UpdateStatus(ParticipantStatus status)
        {
            Status = status;
        }

        public void UpdateRole(ParticipantRole role)
        {
            Role = role;
        }
    }

    public enum ParticipantRole
    {
        Organizer,
        Moderator,
        Speaker,
        Attendee
    }

    public enum ParticipantStatus
    {
        Registered,
        Confirmed,
        CheckedIn,
        Cancelled,
        NoShow
    }
} 