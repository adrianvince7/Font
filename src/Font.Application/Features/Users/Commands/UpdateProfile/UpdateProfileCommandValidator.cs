using FluentValidation;

namespace Font.Application.Features.Users.Commands.UpdateProfile
{
    public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
    {
        public UpdateProfileCommandValidator()
        {
            RuleFor(v => v.UserId)
                .NotEmpty().WithMessage("User ID is required");

            RuleFor(v => v.DisplayName)
                .MaximumLength(100).WithMessage("Display name must not exceed 100 characters");

            RuleFor(v => v.Bio)
                .MaximumLength(500).WithMessage("Bio must not exceed 500 characters");

            RuleFor(v => v.AvatarUrl)
                .Must(uri => string.IsNullOrEmpty(uri) || Uri.TryCreate(uri, UriKind.Absolute, out _))
                .WithMessage("Avatar URL must be a valid URL");

            RuleForEach(v => v.Preferences)
                .Must(kvp => !string.IsNullOrEmpty(kvp.Key) && !string.IsNullOrEmpty(kvp.Value))
                .WithMessage("Preference key and value must not be empty");
        }
    }
} 