using System;

namespace C969.Controllers
{
    public class TimeZoneHandler
    {
        public static (DateTime, DateTime) ConvertToEst(DateTime startLocal, DateTime endLocal, TimeZoneInfo userTimeZone)
        {
            // Ensure the appointment start and end times are treated as local times relative to the user's time zone without any predefined kind
            startLocal = DateTime.SpecifyKind(startLocal, DateTimeKind.Unspecified);
            endLocal = DateTime.SpecifyKind(endLocal, DateTimeKind.Unspecified);

            // Convert the appointment start and end times to EST
            TimeZoneInfo estTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime estStart = TimeZoneInfo.ConvertTime(startLocal, userTimeZone, estTimeZone);
            DateTime estEnd = TimeZoneInfo.ConvertTime(endLocal, userTimeZone, estTimeZone);

            return (estStart, estEnd);
        }
    }
}