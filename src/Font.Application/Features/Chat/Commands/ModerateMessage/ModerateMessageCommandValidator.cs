using FluentValidation;

namespace Font.Application.Features.Chat.Commands.ModerateMessage
{
    public class ModerateMessageCommandValidator : AbstractValidator<ModerateMessageCommand>
    {
        public ModerateMessageCommandValidator()
        {
            RuleFor(v => v.MessageId)
                .NotEmpty().WithMessage("Message ID is required");

            RuleFor(v => v.ModeratorId)
                .NotEmpty().WithMessage("Moderator ID is required");

            RuleFor(v => v.Reason)
                .NotEmpty().WithMessage("Moderation reason is required")
                .MaximumLength(500).WithMessage("Reason must not exceed 500 characters");
        }
    }
} 