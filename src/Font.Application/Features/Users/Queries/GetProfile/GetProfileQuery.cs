using Font.Application.Common.Interfaces;
using Font.Application.Common.Models;
using MediatR;

namespace Font.Application.Features.Users.Queries.GetProfile
{
    public record UserProfileDto
    {
        public string UserId { get; init; } = string.Empty;
        public string? DisplayName { get; init; }
        public string? Bio { get; init; }
        public string? AvatarUrl { get; init; }
        public Dictionary<string, string> Preferences { get; init; } = new();
    }

    public record GetProfileQuery : IRequest<Result<UserProfileDto>>
    {
        public string UserId { get; init; } = string.Empty;
    }

    public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, Result<UserProfileDto>>
    {
        private readonly IUserService _userService;

        public GetProfileQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Result<UserProfileDto>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            var profile = await _userService.GetProfileAsync(request.UserId);

            return profile != null 
                ? Result<UserProfileDto>.Success(profile) 
                : Result<UserProfileDto>.Failure("Profile not found");
        }
    }
} 