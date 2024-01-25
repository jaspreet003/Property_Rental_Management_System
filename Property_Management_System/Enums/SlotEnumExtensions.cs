using System;
namespace Property_Management_System.Enums;
public static class SlotEnumExtensions
{
    public static string ToTimeString(this SlotEnum slot)
    {
        // The start time of the first slot.
        TimeSpan startTime = new TimeSpan(9, 0, 0);

        // Calculate the start time of the given slot.
        TimeSpan slotStartTime = startTime.Add(TimeSpan.FromMinutes(30 * ((int)slot - 1)));

        // Calculate the end time of the given slot.
        TimeSpan slotEndTime = slotStartTime.Add(TimeSpan.FromMinutes(30));

        // Return the time range of the slot.
        return $"{slotStartTime:hh\\:mm} - {slotEndTime:hh\\:mm}";
    }
}