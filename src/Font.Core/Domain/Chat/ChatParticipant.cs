using System;

namespace Font.Core.Domain.Chat
{
    public class ChatParticipant
    {
        public Guid Id { get; private set; }
        public Guid ChatRoomId { get; private set; }
        public Guid UserId { get; private set; }
        public ParticipantRole Role { get; private set; }
        public DateTime JoinedAt { get; private set; }
        public DateTime? LastReadAt { get; private set; }
        public bool IsMuted { get; private set; }
        public DateTime? MutedUntil { get; private set; }
        public bool IsBlocked { get; private set; }

        private ChatParticipant() { } // For EF Core

        public static ChatParticipant Create(Guid chatRoomId, Guid userId, ParticipantRole role = ParticipantRole.Member)
        {
            return new ChatParticipant
            {
                Id = Guid.NewGuid(),
                ChatRoomId = chatRoomId,
                UserId = userId,
                Role = role,
                JoinedAt = DateTime.UtcNow,
                LastReadAt = DateTime.UtcNow,
                IsMuted = false,
                IsBlocked = false
            };
        }

        public void UpdateRole(ParticipantRole newRole)
        {
            Role = newRole;
        }

        public void UpdateLastRead()
        {
            LastReadAt = DateTime.UtcNow;
        }

        public void Mute(DateTime? until = null)
        {
            IsMuted = true;
            MutedUntil = until;
        }

        public void Unmute()
        {
            IsMuted = false;
            MutedUntil = null;
        }

        public void Block()
        {
            IsBlocked = true;
        }

        public void Unblock()
        {
            IsBlocked = false;
        }

        public bool IsMutedNow()
        {
            if (!IsMuted) return false;
            if (!MutedUntil.HasValue) return true;
            return DateTime.UtcNow <= MutedUntil.Value;
        }
    }

    public enum ParticipantRole
    {
        Owner,
        Admin,
        Moderator,
        Member,
        Guest
    }
} 