using System;
using System.Collections.Generic;

namespace Font.Core.Domain.Chat
{
    public class ChatMessage
    {
        public Guid Id { get; private set; }
        public Guid ChatRoomId { get; private set; }
        public Guid SenderId { get; private set; }
        public string Content { get; private set; }
        public DateTime SentAt { get; private set; }
        public DateTime? EditedAt { get; private set; }
        public MessageType Type { get; private set; }
        public List<MessageReaction> Reactions { get; private set; }
        public bool IsDeleted { get; private set; }
        public MessageStatus Status { get; private set; }
        public List<MessageAttachment> Attachments { get; private set; }

        private ChatMessage() { } // For EF Core

        public static ChatMessage Create(
            Guid chatRoomId,
            Guid senderId,
            string content,
            MessageType type = MessageType.Text,
            List<MessageAttachment> attachments = null)
        {
            return new ChatMessage
            {
                Id = Guid.NewGuid(),
                ChatRoomId = chatRoomId,
                SenderId = senderId,
                Content = content,
                SentAt = DateTime.UtcNow,
                Type = type,
                Reactions = new List<MessageReaction>(),
                IsDeleted = false,
                Status = MessageStatus.Sent,
                Attachments = attachments ?? new List<MessageAttachment>()
            };
        }

        public void Edit(string newContent)
        {
            if (IsDeleted)
                throw new InvalidOperationException("Cannot edit a deleted message.");

            Content = newContent;
            EditedAt = DateTime.UtcNow;
        }

        public void Delete()
        {
            IsDeleted = true;
            Content = "[Message deleted]";
            Attachments.Clear();
        }

        public void AddReaction(MessageReaction reaction)
        {
            if (IsDeleted)
                throw new InvalidOperationException("Cannot react to a deleted message.");

            Reactions.Add(reaction);
        }

        public void RemoveReaction(Guid userId, string reactionType)
        {
            var reaction = Reactions.Find(r => r.UserId == userId && r.Type == reactionType);
            if (reaction != null)
            {
                Reactions.Remove(reaction);
            }
        }

        public void UpdateStatus(MessageStatus status)
        {
            Status = status;
        }

        public void AddAttachment(MessageAttachment attachment)
        {
            if (IsDeleted)
                throw new InvalidOperationException("Cannot add attachment to a deleted message.");

            Attachments.Add(attachment);
        }
    }

    public enum MessageType
    {
        Text,
        Image,
        File,
        Audio,
        Video,
        System
    }

    public enum MessageStatus
    {
        Sending,
        Sent,
        Delivered,
        Read,
        Failed
    }
} 