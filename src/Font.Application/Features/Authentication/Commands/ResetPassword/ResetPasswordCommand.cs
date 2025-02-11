using Font.Application.Common.Models;
using MediatR;

namespace Font.Application.Features.Authentication.Commands.ResetPassword
{
    public record ResetPasswordCommand : IRequest<Result>
    {
        public string Email { get; init; } = string.Empty;
    }

    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result>
    {
        private readonly IAuthenticationService _authenticationService;

        public ResetPasswordCommandHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var success = await _authenticationService.ResetPasswordAsync(request.Email);

            return success 
                ? Result.Success() 
                : Result.Failure("Failed to initiate password reset");
        }
    }
} 