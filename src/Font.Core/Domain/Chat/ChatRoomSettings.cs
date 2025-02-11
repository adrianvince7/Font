using System;

namespace Font.Core.Domain.Chat
{
    public class ChatRoomSettings
    {
        public Guid Id { get; private set; }
        public Guid ChatRoomId { get; private set; }
        public bool AllowAnonymousMessages { get; private set; }
        public bool RequireModeration { get; private set; }
        public int MaxParticipants { get; private set; }
        public bool EnableReactions { get; private set; }
        public bool EnableAttachments { get; private set; }
        public long MaxAttachmentSize { get; private set; }
        public string[] AllowedAttachmentTypes { get; private set; }
        public bool EnableHistory { get; private set; }
        public int MessageRetentionDays { get; private set; }
        public bool EnableReadReceipts { get; private set; }
        public bool EnableTypingIndicators { get; private set; }
        public string[] BannedWords { get; private set; }
        public MessageFilterLevel FilterLevel { get; private set; }

        private ChatRoomSettings()
        {
            Id = Guid.NewGuid();
            AllowAnonymousMessages = false;
            RequireModeration = false;
            MaxParticipants = 100;
            EnableReactions = true;
            EnableAttachments = true;
            MaxAttachmentSize = 10 * 1024 * 1024; // 10MB
            AllowedAttachmentTypes = new[] { "image/*", "audio/*", "video/*", "application/pdf" };
            EnableHistory = true;
            MessageRetentionDays = 30;
            EnableReadReceipts = true;
            EnableTypingIndicators = true;
            BannedWords = Array.Empty<string>();
            FilterLevel = MessageFilterLevel.Medium;
        }

        public static ChatRoomSettings Create(Guid chatRoomId)
        {
            return new ChatRoomSettings { ChatRoomId = chatRoomId };
        }

        public void UpdatePrivacySettings(bool allowAnonymousMessages, bool requireModeration)
        {
            AllowAnonymousMessages = allowAnonymousMessages;
            RequireModeration = requireModeration;
        }

        public void UpdateParticipantLimit(int maxParticipants)
        {
            if (maxParticipants < 2)
                throw new ArgumentException("Maximum participants must be at least 2.");

            MaxParticipants = maxParticipants;
        }

        public void UpdateMessageFeatures(bool enableReactions, bool enableReadReceipts, bool enableTypingIndicators)
        {
            EnableReactions = enableReactions;
            EnableReadReceipts = enableReadReceipts;
            EnableTypingIndicators = enableTypingIndicators;
        }

        public void UpdateAttachmentSettings(bool enableAttachments, long maxSize, string[] allowedTypes)
        {
            EnableAttachments = enableAttachments;
            MaxAttachmentSize = maxSize;
            AllowedAttachmentTypes = allowedTypes ?? Array.Empty<string>();
        }

        public void UpdateHistorySettings(bool enableHistory, int retentionDays)
        {
            EnableHistory = enableHistory;
            MessageRetentionDays = retentionDays;
        }

        public void UpdateContentFilter(string[] bannedWords, MessageFilterLevel filterLevel)
        {
            BannedWords = bannedWords ?? Array.Empty<string>();
            FilterLevel = filterLevel;
        }
    }

    public enum MessageFilterLevel
    {
        None,
        Low,
        Medium,
        High,
        Strict
    }
} 