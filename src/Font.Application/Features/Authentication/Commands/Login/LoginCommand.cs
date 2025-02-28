using Font.Application.Common.Models;
using MediatR;

namespace Font.Application.Features.Authentication.Commands.Login
{
    public record LoginCommand : IRequest<Result<string>>
    {
        public string Email { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<string>>
    {
        private readonly IAuthenticationService _authenticationService;

        public LoginCommandHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var (success, token) = await _authenticationService.LoginAsync(
                request.Email,
                request.Password);

            return success 
                ? Result<string>.Success(token) 
                : Result<string>.Failure("Invalid credentials");
        }
    }
} 