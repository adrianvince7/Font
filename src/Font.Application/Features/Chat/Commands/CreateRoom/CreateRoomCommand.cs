using Font.Application.Common.Models;
using MediatR;

namespace Font.Application.Features.Chat.Commands.CreateRoom
{
    public record CreateRoomCommand : IRequest<Result<string>>
    {
        public string Name { get; init; } = string.Empty;
        public string CreatorId { get; init; } = string.Empty;
        public List<string> ParticipantIds { get; init; } = new();
        public bool IsPrivate { get; init; }
        public Dictionary<string, string> Settings { get; init; } = new();
    }

    public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, Result<string>>
    {
        private readonly IChatService _chatService;

        public CreateRoomCommandHandler(IChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task<Result<string>> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            var (success, roomId) = await _chatService.CreateRoomAsync(
                request.Name,
                request.CreatorId,
                request.ParticipantIds,
                request.IsPrivate,
                request.Settings);

            return success 
                ? Result<string>.Success(roomId) 
                : Result<string>.Failure("Failed to create chat room");
        }
    }
} 