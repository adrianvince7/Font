using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Font.Core.Domain.Chat;

namespace Font.Core.Interfaces
{
    public interface IChatRepository
    {
        // ChatRoom operations
        Task<ChatRoom> GetChatRoomByIdAsync(Guid id);
        Task<IEnumerable<ChatRoom>> GetChatRoomsByUserIdAsync(Guid userId);
        Task<IEnumerable<ChatRoom>> GetPublicChatRoomsAsync();
        Task<ChatRoom> CreateChatRoomAsync(ChatRoom chatRoom);
        Task UpdateChatRoomAsync(ChatRoom chatRoom);
        Task DeleteChatRoomAsync(Guid id);

        // Message operations
        Task<ChatMessage> GetMessageByIdAsync(Guid id);
        Task<IEnumerable<ChatMessage>> GetMessagesByChatRoomIdAsync(Guid chatRoomId, int skip = 0, int take = 50);
        Task<ChatMessage> AddMessageAsync(ChatMessage message);
        Task UpdateMessageAsync(ChatMessage message);
        Task DeleteMessageAsync(Guid id);

        // Participant operations
        Task<ChatParticipant> GetParticipantAsync(Guid chatRoomId, Guid userId);
        Task<IEnumerable<ChatParticipant>> GetParticipantsByChatRoomIdAsync(Guid chatRoomId);
        Task AddParticipantAsync(ChatParticipant participant);
        Task UpdateParticipantAsync(ChatParticipant participant);
        Task RemoveParticipantAsync(Guid chatRoomId, Guid userId);

        // Reaction operations
        Task<IEnumerable<MessageReaction>> GetReactionsByMessageIdAsync(Guid messageId);
        Task AddReactionAsync(MessageReaction reaction);
        Task RemoveReactionAsync(Guid messageId, Guid userId, string type);

        // Attachment operations
        Task<MessageAttachment> GetAttachmentByIdAsync(Guid id);
        Task<IEnumerable<MessageAttachment>> GetAttachmentsByMessageIdAsync(Guid messageId);
        Task AddAttachmentAsync(MessageAttachment attachment);
        Task DeleteAttachmentAsync(Guid id);

        // Settings operations
        Task<ChatRoomSettings> GetChatRoomSettingsAsync(Guid chatRoomId);
        Task UpdateChatRoomSettingsAsync(ChatRoomSettings settings);

        // Search operations
        Task<IEnumerable<ChatMessage>> SearchMessagesAsync(Guid chatRoomId, string searchTerm);
        Task<IEnumerable<ChatRoom>> SearchChatRoomsAsync(string searchTerm);

        // Statistics
        Task<int> GetUnreadMessageCountAsync(Guid chatRoomId, Guid userId);
        Task<DateTime?> GetLastActivityAsync(Guid chatRoomId);
        Task<int> GetParticipantCountAsync(Guid chatRoomId);
    }
} 