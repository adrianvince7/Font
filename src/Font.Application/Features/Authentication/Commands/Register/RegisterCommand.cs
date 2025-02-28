using Font.Application.Common.Models;
using MediatR;

namespace Font.Application.Features.Authentication.Commands.Register
{
    public record RegisterCommand : IRequest<Result<string>>
    {
        public string Username { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<string>>
    {
        private readonly IAuthenticationService _authenticationService;

        public RegisterCommandHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<Result<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var (success, message) = await _authenticationService.RegisterAsync(
                request.Username,
                request.Email,
                request.Password);

            return success 
                ? Result<string>.Success(message) 
                : Result<string>.Failure(message);
        }
    }
} 