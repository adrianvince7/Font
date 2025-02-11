using System;

namespace Font.Core.Domain.User
{
    public class EmergencyContact
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Relationship { get; private set; }
        public bool IsPrimaryContact { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private EmergencyContact() { } // For EF Core

        public static EmergencyContact Create(string name, string phoneNumber, string relationship, bool isPrimaryContact = false)
        {
            return new EmergencyContact
            {
                Id = Guid.NewGuid(),
                Name = name,
                PhoneNumber = phoneNumber,
                Relationship = relationship,
                IsPrimaryContact = isPrimaryContact,
                CreatedAt = DateTime.UtcNow
            };
        }

        public void MakePrimary()
        {
            IsPrimaryContact = true;
        }

        public void RemovePrimary()
        {
            IsPrimaryContact = false;
        }
    }
} 