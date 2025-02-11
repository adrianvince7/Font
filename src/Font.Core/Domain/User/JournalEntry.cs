using System;

namespace Font.Core.Domain.User
{
    public class JournalEntry
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Content { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public Mood Mood { get; private set; }
        public bool IsPrivate { get; private set; }
        public string[] Tags { get; private set; }

        private JournalEntry() { } // For EF Core

        public static JournalEntry Create(string title, string content, Mood mood, bool isPrivate = true, string[] tags = null)
        {
            return new JournalEntry
            {
                Id = Guid.NewGuid(),
                Title = title,
                Content = content,
                CreatedAt = DateTime.UtcNow,
                Mood = mood,
                IsPrivate = isPrivate,
                Tags = tags ?? Array.Empty<string>()
            };
        }

        public void UpdateContent(string newContent)
        {
            Content = newContent;
        }

        public void UpdateMood(Mood newMood)
        {
            Mood = newMood;
        }

        public void UpdatePrivacy(bool isPrivate)
        {
            IsPrivate = isPrivate;
        }

        public void UpdateTags(string[] newTags)
        {
            Tags = newTags ?? Array.Empty<string>();
        }
    }

    public enum Mood
    {
        VeryGood,
        Good,
        Neutral,
        Challenging,
        Difficult,
        Crisis
    }
} 