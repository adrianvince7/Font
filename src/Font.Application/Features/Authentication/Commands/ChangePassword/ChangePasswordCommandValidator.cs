using FluentValidation;

namespace Font.Application.Features.Authentication.Commands.ChangePassword
{
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(v => v.UserId)
                .NotEmpty().WithMessage("User ID is required");

            RuleFor(v => v.CurrentPassword)
                .NotEmpty().WithMessage("Current password is required");

            RuleFor(v => v.NewPassword)
                .NotEmpty().WithMessage("New password is required")
                .MinimumLength(8).WithMessage("New password must be at least 8 characters")
                .Matches("[A-Z]").WithMessage("New password must contain at least one uppercase letter")
                .Matches("[a-z]").WithMessage("New password must contain at least one lowercase letter")
                .Matches("[0-9]").WithMessage("New password must contain at least one number")
                .Matches("[^a-zA-Z0-9]").WithMessage("New password must contain at least one special character")
                .NotEqual(v => v.CurrentPassword).WithMessage("New password must be different from current password");

            RuleFor(v => v.ConfirmNewPassword)
                .NotEmpty().WithMessage("Password confirmation is required")
                .Equal(v => v.NewPassword).WithMessage("Passwords do not match");
        }
    }
} 