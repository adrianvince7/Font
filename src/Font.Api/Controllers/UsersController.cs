using Font.Application.Features.Users.Commands.UpdateProfile;
using Font.Application.Features.Users.Queries.GetProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Font.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetProfile(string userId)
        {
            var query = new GetProfileQuery { UserId = userId };
            var result = await _mediator.Send(query);
            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateProfile(string userId, UpdateProfileCommand command)
        {
            if (userId != command.UserId)
            {
                return BadRequest("User ID mismatch");
            }

            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpGet("{userId}/preferences")]
        public async Task<IActionResult> GetPreferences(string userId)
        {
            var query = new GetProfileQuery { UserId = userId };
            var result = await _mediator.Send(query);
            return result.IsSuccess ? Ok(result.Value?.Preferences) : NotFound(result.Error);
        }
    }
} 