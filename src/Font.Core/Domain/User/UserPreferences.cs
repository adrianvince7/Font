using System;

namespace Font.Core.Domain.User
{
    public class UserPreferences
    {
        public Guid Id { get; private set; }
        public bool EnableNotifications { get; private set; }
        public bool EnableLocationServices { get; private set; }
        public bool EnableDailyReminders { get; private set; }
        public TimeSpan? DailyReminderTime { get; private set; }
        public bool ShareProgress { get; private set; }
        public string[] PreferredMeetingTypes { get; private set; }
        public int MaxTravelDistance { get; private set; }
        public string PreferredLanguage { get; private set; }
        public ThemePreference ThemePreference { get; private set; }

        private UserPreferences()
        {
            Id = Guid.NewGuid();
            EnableNotifications = true;
            EnableLocationServices = true;
            EnableDailyReminders = true;
            DailyReminderTime = new TimeSpan(20, 0, 0); // 8:00 PM
            ShareProgress = false;
            PreferredMeetingTypes = new[] { "In-Person", "Virtual" };
            MaxTravelDistance = 25; // Default 25 miles/km
            PreferredLanguage = "en";
            ThemePreference = ThemePreference.System;
        }

        public void UpdateNotificationPreferences(bool enableNotifications, bool enableDailyReminders, TimeSpan? dailyReminderTime)
        {
            EnableNotifications = enableNotifications;
            EnableDailyReminders = enableDailyReminders;
            DailyReminderTime = dailyReminderTime;
        }

        public void UpdateLocationPreferences(bool enableLocationServices, int maxTravelDistance)
        {
            EnableLocationServices = enableLocationServices;
            MaxTravelDistance = maxTravelDistance;
        }

        public void UpdatePrivacyPreferences(bool shareProgress)
        {
            ShareProgress = shareProgress;
        }

        public void UpdatePreferredMeetingTypes(string[] meetingTypes)
        {
            PreferredMeetingTypes = meetingTypes ?? Array.Empty<string>();
        }

        public void UpdateLanguagePreference(string language)
        {
            PreferredLanguage = language;
        }

        public void UpdateThemePreference(ThemePreference theme)
        {
            ThemePreference = theme;
        }
    }

    public enum ThemePreference
    {
        Light,
        Dark,
        System
    }
} 