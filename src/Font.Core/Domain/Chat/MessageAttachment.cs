using System;

namespace Font.Core.Domain.Chat
{
    public class MessageAttachment
    {
        public Guid Id { get; private set; }
        public Guid MessageId { get; private set; }
        public string FileName { get; private set; }
        public string FileUrl { get; private set; }
        public string ContentType { get; private set; }
        public long FileSize { get; private set; }
        public AttachmentType Type { get; private set; }
        public DateTime UploadedAt { get; private set; }
        public string ThumbnailUrl { get; private set; }
        public AttachmentMetadata Metadata { get; private set; }

        private MessageAttachment() { } // For EF Core

        public static MessageAttachment Create(
            Guid messageId,
            string fileName,
            string fileUrl,
            string contentType,
            long fileSize,
            AttachmentType type,
            string thumbnailUrl = null,
            AttachmentMetadata metadata = null)
        {
            return new MessageAttachment
            {
                Id = Guid.NewGuid(),
                MessageId = messageId,
                FileName = fileName,
                FileUrl = fileUrl,
                ContentType = contentType,
                FileSize = fileSize,
                Type = type,
                UploadedAt = DateTime.UtcNow,
                ThumbnailUrl = thumbnailUrl,
                Metadata = metadata ?? new AttachmentMetadata()
            };
        }

        public void UpdateMetadata(AttachmentMetadata metadata)
        {
            Metadata = metadata;
        }

        public void UpdateThumbnail(string thumbnailUrl)
        {
            ThumbnailUrl = thumbnailUrl;
        }
    }

    public class AttachmentMetadata
    {
        public int? Width { get; set; }
        public int? Height { get; set; }
        public TimeSpan? Duration { get; set; }
        public string Description { get; set; }
        public string[] Tags { get; set; }
    }

    public enum AttachmentType
    {
        Image,
        Video,
        Audio,
        Document,
        Other
    }
} 