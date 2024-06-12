using System;

namespace C969.Controllers
{
    /// <summary>
    /// Class to handle time zone conversion between the user's time zone and EST.
    /// </summary>
    public class TimeZoneHandler
    {
        private static readonly TimeZoneInfo _estTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

        /// <summary>
        /// Converts a start and end time to EST
        /// </summary>
        /// <param name="startLocal"></param>
        /// <param name="endLocal"></param>
        /// <param name="nowLocal"></param>
        /// <param name="userTimeZone"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Converts a DateTime object to the user's time zone
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="userTimeZone"></param>
        /// <returns></returns>
        public static DateTime ConvertToUserTimeZone(DateTime dateTime, TimeZoneInfo userTimeZone)
        {
            dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTime(dateTime, _estTimeZone, userTimeZone);
        }
    }
}