using System;
using System.Collections.Generic;

namespace Font.Core.Domain.Meeting
{
    public class Meeting
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Location { get; private set; }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public MeetingType Type { get; private set; }
        public string VirtualMeetingUrl { get; private set; }
        public string VirtualMeetingPassword { get; private set; }
        public bool IsRecurring { get; private set; }
        public RecurrencePattern RecurrencePattern { get; private set; }
        public int MaxParticipants { get; private set; }
        public List<MeetingParticipant> Participants { get; private set; }
        public string[] Tags { get; private set; }
        public bool IsPrivate { get; private set; }
        public Guid OrganizerId { get; private set; }

        private Meeting() { } // For EF Core

        public static Meeting Create(
            string name,
            string description,
            string location,
            double latitude,
            double longitude,
            DateTime startTime,
            DateTime endTime,
            MeetingType type,
            Guid organizerId,
            bool isRecurring = false,
            RecurrencePattern recurrencePattern = null,
            int maxParticipants = 0,
            string virtualMeetingUrl = null,
            string virtualMeetingPassword = null,
            bool isPrivate = false,
            string[] tags = null)
        {
            return new Meeting
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = description,
                Location = location,
                Latitude = latitude,
                Longitude = longitude,
                StartTime = startTime,
                EndTime = endTime,
                Type = type,
                OrganizerId = organizerId,
                IsRecurring = isRecurring,
                RecurrencePattern = recurrencePattern,
                MaxParticipants = maxParticipants,
                VirtualMeetingUrl = virtualMeetingUrl,
                VirtualMeetingPassword = virtualMeetingPassword,
                IsPrivate = isPrivate,
                Tags = tags ?? Array.Empty<string>(),
                Participants = new List<MeetingParticipant>()
            };
        }

        public void AddParticipant(MeetingParticipant participant)
        {
            if (MaxParticipants > 0 && Participants.Count >= MaxParticipants)
            {
                throw new InvalidOperationException("Meeting has reached maximum participants.");
            }

            Participants.Add(participant);
        }

        public void RemoveParticipant(Guid participantId)
        {
            var participant = Participants.Find(p => p.UserId == participantId);
            if (participant != null)
            {
                Participants.Remove(participant);
            }
        }

        public void UpdateMeetingDetails(
            string name = null,
            string description = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            string[] tags = null)
        {
            if (name != null) Name = name;
            if (description != null) Description = description;
            if (startTime.HasValue) StartTime = startTime.Value;
            if (endTime.HasValue) EndTime = endTime.Value;
            if (tags != null) Tags = tags;
        }

        public void UpdateLocation(string location, double latitude, double longitude)
        {
            Location = location;
            Latitude = latitude;
            Longitude = longitude;
        }

        public void UpdateVirtualMeetingDetails(string url, string password)
        {
            VirtualMeetingUrl = url;
            VirtualMeetingPassword = password;
        }
    }

    public enum MeetingType
    {
        InPerson,
        Virtual,
        Hybrid
    }
} 