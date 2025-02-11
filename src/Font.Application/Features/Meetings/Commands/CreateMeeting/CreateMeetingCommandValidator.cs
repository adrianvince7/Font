using FluentValidation;

namespace Font.Application.Features.Meetings.Commands.CreateMeeting
{
    public class CreateMeetingCommandValidator : AbstractValidator<CreateMeetingCommand>
    {
        public CreateMeetingCommandValidator()
        {
            RuleFor(v => v.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

            RuleFor(v => v.Description)
                .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters");

            RuleFor(v => v.StartTime)
                .NotEmpty().WithMessage("Start time is required")
                .Must(startTime => startTime > DateTime.UtcNow)
                .WithMessage("Start time must be in the future");

            RuleFor(v => v.EndTime)
                .NotEmpty().WithMessage("End time is required")
                .Must((command, endTime) => endTime > command.StartTime)
                .WithMessage("End time must be after start time");

            RuleFor(v => v.HostId)
                .NotEmpty().WithMessage("Host ID is required");

            RuleFor(v => v.ParticipantIds)
                .Must(list => list.Count <= 100)
                .WithMessage("Maximum of 100 participants allowed");

            RuleForEach(v => v.Settings)
                .Must(kvp => !string.IsNullOrEmpty(kvp.Key) && !string.IsNullOrEmpty(kvp.Value))
                .WithMessage("Setting key and value must not be empty");
        }
    }
} 