using Font.Application.Common.Interfaces;
using Font.Application.Common.Models;
using Font.Application.Features.Meetings.Queries.GetMeeting;
using MediatR;

namespace Font.Application.Features.Meetings.Queries.SearchMeetings
{
    public record SearchMeetingsQuery : IRequest<Result<PaginatedList<MeetingDto>>>
    {
        public string SearchTerm { get; init; } = string.Empty;
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }

    public class SearchMeetingsQueryHandler : IRequestHandler<SearchMeetingsQuery, Result<PaginatedList<MeetingDto>>>
    {
        private readonly IMeetingService _meetingService;

        public SearchMeetingsQueryHandler(IMeetingService meetingService)
        {
            _meetingService = meetingService;
        }

        public async Task<Result<PaginatedList<MeetingDto>>> Handle(SearchMeetingsQuery request, CancellationToken cancellationToken)
        {
            var meetings = await _meetingService.SearchMeetingsAsync(
                request.SearchTerm,
                request.PageNumber,
                request.PageSize);

            var totalCount = meetings.Count;
            var paginatedList = new PaginatedList<MeetingDto>(
                meetings,
                totalCount,
                request.PageNumber,
                request.PageSize);

            return Result<PaginatedList<MeetingDto>>.Success(paginatedList);
        }
    }
} 