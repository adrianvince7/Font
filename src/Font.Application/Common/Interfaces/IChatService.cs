using Font.Application.Features.Chat.Queries.GetMessages;

namespace Font.Application.Common.Interfaces
{
    public interface IChatService
    {
        Task<(bool success, string messageId)> SendMessageAsync(
            string roomId,
            string senderId,
            string content,
            string? replyToMessageId,
            List<string> mentions,
            Dictionary<string, string> metadata);

        Task<(bool success, string roomId)> CreateRoomAsync(
            string name,
            string creatorId,
            List<string> participantIds,
            bool isPrivate,
            Dictionary<string, string> settings);

        Task<ChatMessageDto?> GetMessageAsync(string messageId);
        Task<List<ChatMessageDto>> GetMessagesAsync(string roomId, int page, int pageSize);
        Task<bool> DeleteMessageAsync(string messageId, string userId);
        Task<bool> ModerateMessageAsync(string messageId, string moderatorId, string reason);
        Task<bool> JoinRoomAsync(string roomId, string userId);
        Task<bool> LeaveRoomAsync(string roomId, string userId);
        Task<List<ChatRoomDto>> GetUserRoomsAsync(string userId);
    }
} 