using FluentValidation;

namespace Font.Application.Features.Chat.Commands.CreateRoom
{
    public class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
    {
        public CreateRoomCommandValidator()
        {
            RuleFor(v => v.Name)
                .NotEmpty().WithMessage("Room name is required")
                .MaximumLength(100).WithMessage("Room name must not exceed 100 characters");

            RuleFor(v => v.CreatorId)
                .NotEmpty().WithMessage("Creator ID is required");

            RuleFor(v => v.ParticipantIds)
                .Must(list => list.Count <= 100)
                .WithMessage("Maximum of 100 participants allowed");

            RuleForEach(v => v.Settings)
                .Must(kvp => !string.IsNullOrEmpty(kvp.Key) && !string.IsNullOrEmpty(kvp.Value))
                .WithMessage("Setting key and value must not be empty");
        }
    }
} 