using FluentValidation;

namespace Font.Application.Features.Chat.Commands.SendMessage
{
    public class SendMessageCommandValidator : AbstractValidator<SendMessageCommand>
    {
        public SendMessageCommandValidator()
        {
            RuleFor(v => v.RoomId)
                .NotEmpty().WithMessage("Room ID is required");

            RuleFor(v => v.SenderId)
                .NotEmpty().WithMessage("Sender ID is required");

            RuleFor(v => v.Content)
                .NotEmpty().WithMessage("Message content is required")
                .MaximumLength(5000).WithMessage("Message content must not exceed 5000 characters");

            RuleFor(v => v.Mentions)
                .Must(list => list.Count <= 50)
                .WithMessage("Maximum of 50 mentions allowed per message");

            RuleForEach(v => v.Metadata)
                .Must(kvp => !string.IsNullOrEmpty(kvp.Key) && !string.IsNullOrEmpty(kvp.Value))
                .WithMessage("Metadata key and value must not be empty");
        }
    }
} 