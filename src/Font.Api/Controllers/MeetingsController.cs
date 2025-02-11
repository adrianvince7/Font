using Font.Application.Features.Meetings.Commands.CreateMeeting;
using Font.Application.Features.Meetings.Queries.GetMeeting;
using Font.Application.Features.Meetings.Queries.SearchMeetings;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Font.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MeetingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MeetingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMeetingCommand command)
        {
            var result = await _mediator.Send(command);
            return result.IsSuccess 
                ? CreatedAtAction(nameof(Get), new { meetingId = result.Value }, result.Value) 
                : BadRequest(result.Error);
        }

        [HttpGet("{meetingId}")]
        public async Task<IActionResult> Get(string meetingId)
        {
            var query = new GetMeetingQuery { MeetingId = meetingId };
            var result = await _mediator.Send(query);
            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] SearchMeetingsQuery query)
        {
            var result = await _mediator.Send(query);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpPost("{meetingId}/join")]
        public async Task<IActionResult> Join(string meetingId)
        {
            // Get user ID from claims
            var userId = User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var command = new JoinMeetingCommand { MeetingId = meetingId, UserId = userId };
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPost("{meetingId}/leave")]
        public async Task<IActionResult> Leave(string meetingId)
        {
            // Get user ID from claims
            var userId = User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var command = new LeaveMeetingCommand { MeetingId = meetingId, UserId = userId };
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }
    }
} 