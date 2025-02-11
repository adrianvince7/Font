using Font.Application.Common.Models;
using MediatR;

namespace Font.Application.Features.Users.Commands.UpdateProfile
{
    public record UpdateProfileCommand : IRequest<Result>
    {
        public string UserId { get; init; } = string.Empty;
        public string? DisplayName { get; init; }
        public string? Bio { get; init; }
        public string? AvatarUrl { get; init; }
        public Dictionary<string, string> Preferences { get; init; } = new();
    }

    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, Result>
    {
        private readonly IUserService _userService;

        public UpdateProfileCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Result> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var success = await _userService.UpdateProfileAsync(
                request.UserId,
                request.DisplayName,
                request.Bio,
                request.AvatarUrl,
                request.Preferences);

            return success 
                ? Result.Success() 
                : Result.Failure("Failed to update profile");
        }
    }
} 