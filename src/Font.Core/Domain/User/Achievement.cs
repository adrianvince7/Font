using System;

namespace Font.Core.Domain.User
{
    public class Achievement
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime UnlockedAt { get; private set; }
        public AchievementType Type { get; private set; }
        public int Milestone { get; private set; }

        private Achievement() { } // For EF Core

        public static Achievement Create(string title, string description, AchievementType type, int milestone)
        {
            return new Achievement
            {
                Id = Guid.NewGuid(),
                Title = title,
                Description = description,
                UnlockedAt = DateTime.UtcNow,
                Type = type,
                Milestone = milestone
            };
        }
    }

    public enum AchievementType
    {
        DaysSober,
        MeetingsAttended,
        JournalEntries,
        CommunitySupport,
        PersonalGoal
    }
} 