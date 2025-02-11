using System;
using System.Collections.Generic;

namespace Font.Core.Domain.User
{
    public class SobrietyProgress
    {
        public Guid Id { get; private set; }
        public DateTime SobrietyStartDate { get; private set; }
        public int CurrentStreak { get; private set; }
        public int LongestStreak { get; private set; }
        public List<Achievement> Achievements { get; private set; }
        public List<JournalEntry> JournalEntries { get; private set; }
        public DateTime LastCheckedIn { get; private set; }

        private SobrietyProgress()
        {
            Id = Guid.NewGuid();
            SobrietyStartDate = DateTime.UtcNow;
            CurrentStreak = 0;
            LongestStreak = 0;
            Achievements = new List<Achievement>();
            JournalEntries = new List<JournalEntry>();
            LastCheckedIn = DateTime.UtcNow;
        }

        public void CheckIn()
        {
            var today = DateTime.UtcNow.Date;
            var daysSinceLastCheckIn = (today - LastCheckedIn.Date).Days;

            if (daysSinceLastCheckIn == 1)
            {
                CurrentStreak++;
                if (CurrentStreak > LongestStreak)
                {
                    LongestStreak = CurrentStreak;
                }
            }
            else if (daysSinceLastCheckIn > 1)
            {
                CurrentStreak = 1;
            }

            LastCheckedIn = today;
        }

        public void AddJournalEntry(JournalEntry entry)
        {
            JournalEntries.Add(entry);
        }

        public void AddAchievement(Achievement achievement)
        {
            Achievements.Add(achievement);
        }

        public int GetDaysSober()
        {
            return (int)(DateTime.UtcNow - SobrietyStartDate).TotalDays;
        }

        public void Reset()
        {
            SobrietyStartDate = DateTime.UtcNow;
            CurrentStreak = 0;
            LastCheckedIn = DateTime.UtcNow;
        }
    }
} 