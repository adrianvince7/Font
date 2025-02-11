using System;
using System.Collections.Generic;

namespace Font.Core.Domain.User
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Username { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? LastLoginAt { get; private set; }
        public bool IsAnonymous { get; private set; }
        public List<EmergencyContact> EmergencyContacts { get; private set; }
        public SobrietyProgress SobrietyProgress { get; private set; }
        public UserPreferences Preferences { get; private set; }

        private User() { } // For EF Core

        public static User Create(string username, string email, string passwordHash, bool isAnonymous = false)
        {
            return new User
            {
                Id = Guid.NewGuid(),
                Username = username,
                Email = email,
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow,
                IsAnonymous = isAnonymous,
                EmergencyContacts = new List<EmergencyContact>(),
                SobrietyProgress = new SobrietyProgress(),
                Preferences = new UserPreferences()
            };
        }

        public void UpdateLastLogin()
        {
            LastLoginAt = DateTime.UtcNow;
        }

        public void AddEmergencyContact(EmergencyContact contact)
        {
            EmergencyContacts ??= new List<EmergencyContact>();
            EmergencyContacts.Add(contact);
        }

        public void UpdatePreferences(UserPreferences preferences)
        {
            Preferences = preferences;
        }
    }
} 