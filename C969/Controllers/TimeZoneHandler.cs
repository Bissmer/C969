using System;

namespace C969.Controllers
{
    public class TimeZoneHandler
    {
        private static readonly TimeZoneInfo _estTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
        public static (DateTime, DateTime, DateTime) ConvertToEst(DateTime startLocal, DateTime endLocal, DateTime nowLocal, TimeZoneInfo userTimeZone)
        {
            // Ensure the appointment start and end times are treated as local times relative to the user's time zone without any predefined kind
            startLocal = DateTime.SpecifyKind(startLocal, DateTimeKind.Unspecified);
            endLocal = DateTime.SpecifyKind(endLocal, DateTimeKind.Unspecified);
            nowLocal = DateTime.SpecifyKind(nowLocal, DateTimeKind.Unspecified);

            // Convert the appointment start and end times to EST
            
            DateTime estStart = TimeZoneInfo.ConvertTime(startLocal, userTimeZone, _estTimeZone);
            DateTime estEnd = TimeZoneInfo.ConvertTime(endLocal, userTimeZone, _estTimeZone);
            DateTime estNow = TimeZoneInfo.ConvertTime(nowLocal, userTimeZone, _estTimeZone);

            return (estStart, estEnd, estNow);
        }

        public static DateTime ConvertToEst(DateTime dateTime, TimeZoneInfo userTimeZone)
        {
            dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTime(dateTime, userTimeZone, _estTimeZone);
        }

        public static DateTime ConvertToUserTimeZone(DateTime dateTime, TimeZoneInfo userTimeZone)
        {
            dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTime(dateTime, _estTimeZone, userTimeZone);
        }
    }
}