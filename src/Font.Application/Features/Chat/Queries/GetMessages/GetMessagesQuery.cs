using Font.Application.Common.Interfaces;
using Font.Application.Common.Models;
using MediatR;

namespace Font.Application.Features.Chat.Queries.GetMessages
{
    public record ChatMessageDto
    {
        public string MessageId { get; init; } = string.Empty;
        public string RoomId { get; init; } = string.Empty;
        public string SenderId { get; init; } = string.Empty;
        public string Content { get; init; } = string.Empty;
        public string? ReplyToMessageId { get; init; }
        public List<string> Mentions { get; init; } = new();
        public Dictionary<string, string> Metadata { get; init; } = new();
        public DateTime CreatedAt { get; init; }
        public bool IsModerated { get; init; }
        public string? ModeratedBy { get; init; }
        public string? ModerationReason { get; init; }
    }

    public record ChatRoomDto
    {
        public string RoomId { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string CreatorId { get; init; } = string.Empty;
        public List<string> ParticipantIds { get; init; } = new();
        public bool IsPrivate { get; init; }
        public Dictionary<string, string> Settings { get; init; } = new();
        public DateTime CreatedAt { get; init; }
        public DateTime? LastActivityAt { get; init; }
    }

    public record GetMessagesQuery : IRequest<Result<PaginatedList<ChatMessageDto>>>
    {
        public string RoomId { get; init; } = string.Empty;
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 50;
    }

    public class GetMessagesQueryHandler : IRequestHandler<GetMessagesQuery, Result<PaginatedList<ChatMessageDto>>>
    {
        private readonly IChatService _chatService;

        public GetMessagesQueryHandler(IChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task<Result<PaginatedList<ChatMessageDto>>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
        {
            var messages = await _chatService.GetMessagesAsync(
                request.RoomId,
                request.PageNumber,
                request.PageSize);

            var totalCount = messages.Count;
            var paginatedList = new PaginatedList<ChatMessageDto>(
                messages,
                totalCount,
                request.PageNumber,
                request.PageSize);

            return Result<PaginatedList<ChatMessageDto>>.Success(paginatedList);
        }
    }
} 