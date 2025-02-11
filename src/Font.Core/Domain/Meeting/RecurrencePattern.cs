using System;

namespace Font.Core.Domain.Meeting
{
    public class RecurrencePattern
    {
        public Guid Id { get; private set; }
        public RecurrenceType Type { get; private set; }
        public int Interval { get; private set; }
        public DayOfWeek[] DaysOfWeek { get; private set; }
        public int? DayOfMonth { get; private set; }
        public DateTime? StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        public int? OccurrenceCount { get; private set; }

        private RecurrencePattern() { } // For EF Core

        public static RecurrencePattern Create(
            RecurrenceType type,
            int interval = 1,
            DayOfWeek[] daysOfWeek = null,
            int? dayOfMonth = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            int? occurrenceCount = null)
        {
            return new RecurrencePattern
            {
                Id = Guid.NewGuid(),
                Type = type,
                Interval = interval,
                DaysOfWeek = daysOfWeek ?? Array.Empty<DayOfWeek>(),
                DayOfMonth = dayOfMonth,
                StartDate = startDate ?? DateTime.UtcNow,
                EndDate = endDate,
                OccurrenceCount = occurrenceCount
            };
        }

        public bool IsActive(DateTime date)
        {
            if (StartDate.HasValue && date < StartDate.Value)
                return false;

            if (EndDate.HasValue && date > EndDate.Value)
                return false;

            return true;
        }

        public DateTime? GetNextOccurrence(DateTime fromDate)
        {
            if (!IsActive(fromDate))
                return null;

            switch (Type)
            {
                case RecurrenceType.Daily:
                    return fromDate.AddDays(Interval);

                case RecurrenceType.Weekly:
                    if (DaysOfWeek == null || DaysOfWeek.Length == 0)
                        return fromDate.AddDays(7 * Interval);
                    
                    // Find the next day of week
                    var currentDayOfWeek = fromDate.DayOfWeek;
                    var nextDay = DaysOfWeek.FirstOrDefault(d => d > currentDayOfWeek);
                    if (nextDay == default(DayOfWeek))
                    {
                        // If no next day in current week, move to first day of next week
                        nextDay = DaysOfWeek[0];
                        return fromDate.AddDays((7 - (int)currentDayOfWeek) + (int)nextDay + (7 * (Interval - 1)));
                    }
                    return fromDate.AddDays((int)nextDay - (int)currentDayOfWeek);

                case RecurrenceType.Monthly:
                    if (!DayOfMonth.HasValue)
                        return fromDate.AddMonths(Interval);
                    
                    var nextMonth = fromDate.AddMonths(Interval);
                    return new DateTime(nextMonth.Year, nextMonth.Month, DayOfMonth.Value);

                default:
                    return null;
            }
        }
    }

    public enum RecurrenceType
    {
        Daily,
        Weekly,
        Monthly
    }
} 