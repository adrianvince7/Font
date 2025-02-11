using Font.Application.Features.Chat.Commands.CreateRoom;
using Font.Application.Features.Chat.Commands.ModerateMessage;
using Font.Application.Features.Chat.Commands.SendMessage;
using Font.Application.Features.Chat.Queries.GetMessages;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Font.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChatController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("rooms")]
        public async Task<IActionResult> CreateRoom(CreateRoomCommand command)
        {
            var result = await _mediator.Send(command);
            return result.IsSuccess 
                ? CreatedAtAction(nameof(GetMessages), new { roomId = result.Value }, result.Value) 
                : BadRequest(result.Error);
        }

        [HttpGet("rooms/{roomId}/messages")]
        public async Task<IActionResult> GetMessages(string roomId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 50)
        {
            var query = new GetMessagesQuery 
            { 
                RoomId = roomId,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            var result = await _mediator.Send(query);
            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }

        [HttpPost("rooms/{roomId}/messages")]
        public async Task<IActionResult> SendMessage(string roomId, SendMessageCommand command)
        {
            if (roomId != command.RoomId)
            {
                return BadRequest("Room ID mismatch");
            }

            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [Authorize(Roles = "Moderator")]
        [HttpPost("messages/{messageId}/moderate")]
        public async Task<IActionResult> ModerateMessage(string messageId, ModerateMessageCommand command)
        {
            if (messageId != command.MessageId)
            {
                return BadRequest("Message ID mismatch");
            }

            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPost("rooms/{roomId}/join")]
        public async Task<IActionResult> JoinRoom(string roomId)
        {
            // Get user ID from claims
            var userId = User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var command = new JoinRoomCommand { RoomId = roomId, UserId = userId };
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPost("rooms/{roomId}/leave")]
        public async Task<IActionResult> LeaveRoom(string roomId)
        {
            // Get user ID from claims
            var userId = User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var command = new LeaveRoomCommand { RoomId = roomId, UserId = userId };
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }
    }
} 