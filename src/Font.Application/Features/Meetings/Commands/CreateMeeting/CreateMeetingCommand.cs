using Font.Application.Common.Models;
using MediatR;

namespace Font.Application.Features.Meetings.Commands.CreateMeeting
{
    public record CreateMeetingCommand : IRequest<Result<string>>
    {
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public DateTime StartTime { get; init; }
        public DateTime EndTime { get; init; }
        public string HostId { get; init; } = string.Empty;
        public List<string> ParticipantIds { get; init; } = new();
        public bool IsPrivate { get; init; }
        public Dictionary<string, string> Settings { get; init; } = new();
    }

    public class CreateMeetingCommandHandler : IRequestHandler<CreateMeetingCommand, Result<string>>
    {
        private readonly IMeetingService _meetingService;

        public CreateMeetingCommandHandler(IMeetingService meetingService)
        {
            _meetingService = meetingService;
        }

        public async Task<Result<string>> Handle(CreateMeetingCommand request, CancellationToken cancellationToken)
        {
            var (success, meetingId) = await _meetingService.CreateMeetingAsync(
                request.Title,
                request.Description,
                request.StartTime,
                request.EndTime,
                request.HostId,
                request.ParticipantIds,
                request.IsPrivate,
                request.Settings);

            return success 
                ? Result<string>.Success(meetingId) 
                : Result<string>.Failure("Failed to create meeting");
        }
    }
} 