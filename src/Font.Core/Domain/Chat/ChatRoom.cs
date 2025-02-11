using System;
using System.Collections.Generic;

namespace Font.Core.Domain.Chat
{
    public class ChatRoom
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public ChatRoomType Type { get; private set; }
        public bool IsPrivate { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public Guid CreatedBy { get; private set; }
        public List<ChatMessage> Messages { get; private set; }
        public List<ChatParticipant> Participants { get; private set; }
        public ChatRoomSettings Settings { get; private set; }

        private ChatRoom() { } // For EF Core

        public static ChatRoom Create(
            string name,
            string description,
            ChatRoomType type,
            Guid createdBy,
            bool isPrivate = false,
            ChatRoomSettings settings = null)
        {
            return new ChatRoom
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = description,
                Type = type,
                CreatedBy = createdBy,
                IsPrivate = isPrivate,
                CreatedAt = DateTime.UtcNow,
                Messages = new List<ChatMessage>(),
                Participants = new List<ChatParticipant>(),
                Settings = settings ?? new ChatRoomSettings()
            };
        }

        public void AddMessage(ChatMessage message)
        {
            if (!IsParticipant(message.SenderId))
                throw new InvalidOperationException("Sender is not a participant in this chat room.");

            Messages.Add(message);
        }

        public void AddParticipant(ChatParticipant participant)
        {
            if (IsParticipant(participant.UserId))
                throw new InvalidOperationException("User is already a participant.");

            Participants.Add(participant);
        }

        public void RemoveParticipant(Guid userId)
        {
            var participant = Participants.Find(p => p.UserId == userId);
            if (participant != null)
            {
                Participants.Remove(participant);
            }
        }

        public bool IsParticipant(Guid userId)
        {
            return Participants.Exists(p => p.UserId == userId);
        }

        public void UpdateSettings(ChatRoomSettings settings)
        {
            Settings = settings;
        }

        public void UpdateDetails(string name = null, string description = null)
        {
            if (name != null) Name = name;
            if (description != null) Description = description;
        }
    }

    public enum ChatRoomType
    {
        OneOnOne,
        Group,
        Support,
        Anonymous
    }
} 