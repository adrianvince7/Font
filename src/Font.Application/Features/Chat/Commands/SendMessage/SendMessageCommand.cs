using Font.Application.Common.Models;
using MediatR;

namespace Font.Application.Features.Chat.Commands.SendMessage
{
    public record SendMessageCommand : IRequest<Result<string>>
    {
        public string RoomId { get; init; } = string.Empty;
        public string SenderId { get; init; } = string.Empty;
        public string Content { get; init; } = string.Empty;
        public string? ReplyToMessageId { get; init; }
        public List<string> Mentions { get; init; } = new();
        public Dictionary<string, string> Metadata { get; init; } = new();
    }

    public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, Result<string>>
    {
        private readonly IChatService _chatService;

        public SendMessageCommandHandler(IChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task<Result<string>> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            var (success, messageId) = await _chatService.SendMessageAsync(
                request.RoomId,
                request.SenderId,
                request.Content,
                request.ReplyToMessageId,
                request.Mentions,
                request.Metadata);

            return success 
                ? Result<string>.Success(messageId) 
                : Result<string>.Failure("Failed to send message");
        }
    }
} 