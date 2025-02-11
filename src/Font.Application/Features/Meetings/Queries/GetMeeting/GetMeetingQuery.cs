using Font.Application.Common.Interfaces;
using Font.Application.Common.Models;
using MediatR;

namespace Font.Application.Features.Meetings.Queries.GetMeeting
{
    public record MeetingDto
    {
        public string MeetingId { get; init; } = string.Empty;
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public DateTime StartTime { get; init; }
        public DateTime EndTime { get; init; }
        public string HostId { get; init; } = string.Empty;
        public List<string> ParticipantIds { get; init; } = new();
        public bool IsPrivate { get; init; }
        public Dictionary<string, string> Settings { get; init; } = new();
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
    }

    public record GetMeetingQuery : IRequest<Result<MeetingDto>>
    {
        public string MeetingId { get; init; } = string.Empty;
    }

    public class GetMeetingQueryHandler : IRequestHandler<GetMeetingQuery, Result<MeetingDto>>
    {
        private readonly IMeetingService _meetingService;

        public GetMeetingQueryHandler(IMeetingService meetingService)
        {
            _meetingService = meetingService;
        }

        public async Task<Result<MeetingDto>> Handle(GetMeetingQuery request, CancellationToken cancellationToken)
        {
            var meeting = await _meetingService.GetMeetingAsync(request.MeetingId);

            return meeting != null 
                ? Result<MeetingDto>.Success(meeting) 
                : Result<MeetingDto>.Failure("Meeting not found");
        }
    }
} 