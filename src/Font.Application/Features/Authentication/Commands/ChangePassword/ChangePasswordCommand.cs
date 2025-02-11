using Font.Application.Common.Models;
using MediatR;

namespace Font.Application.Features.Authentication.Commands.ChangePassword
{
    public record ChangePasswordCommand : IRequest<Result>
    {
        public string UserId { get; init; } = string.Empty;
        public string CurrentPassword { get; init; } = string.Empty;
        public string NewPassword { get; init; } = string.Empty;
        public string ConfirmNewPassword { get; init; } = string.Empty;
    }

    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result>
    {
        private readonly IAuthenticationService _authenticationService;

        public ChangePasswordCommandHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            if (request.NewPassword != request.ConfirmNewPassword)
            {
                return Result.Failure("New password and confirmation do not match");
            }

            var success = await _authenticationService.ChangePasswordAsync(
                request.UserId,
                request.CurrentPassword,
                request.NewPassword);

            return success 
                ? Result.Success() 
                : Result.Failure("Failed to change password");
        }
    }
} 