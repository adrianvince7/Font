using Font.Application.Common.Models;
using MediatR;

namespace Font.Application.Features.Chat.Commands.ModerateMessage
{
    public record ModerateMessageCommand : IRequest<Result>
    {
        public string MessageId { get; init; } = string.Empty;
        public string ModeratorId { get; init; } = string.Empty;
        public string Reason { get; init; } = string.Empty;
    }

    public class ModerateMessageCommandHandler : IRequestHandler<ModerateMessageCommand, Result>
    {
        private readonly IChatService _chatService;

        public ModerateMessageCommandHandler(IChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task<Result> Handle(ModerateMessageCommand request, CancellationToken cancellationToken)
        {
            var success = await _chatService.ModerateMessageAsync(
                request.MessageId,
                request.ModeratorId,
                request.Reason);

            return success 
                ? Result.Success() 
                : Result.Failure("Failed to moderate message");
        }
    }
} 