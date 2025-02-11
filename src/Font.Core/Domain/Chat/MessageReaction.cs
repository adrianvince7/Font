using System;

namespace Font.Core.Domain.Chat
{
    public class MessageReaction
    {
        public Guid Id { get; private set; }
        public Guid MessageId { get; private set; }
        public Guid UserId { get; private set; }
        public string Type { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private MessageReaction() { } // For EF Core

        public static MessageReaction Create(Guid messageId, Guid userId, string type)
        {
            return new MessageReaction
            {
                Id = Guid.NewGuid(),
                MessageId = messageId,
                UserId = userId,
                Type = type,
                CreatedAt = DateTime.UtcNow
            };
        }

        public void UpdateType(string newType)
        {
            Type = newType;
        }
    }
} 