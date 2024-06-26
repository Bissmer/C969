﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C969.Models
{
    /// <summary>
    /// Class is used to store the count of appointments by month and type
    /// </summary>
    public class AppointmentTypesByMonths
    {
        public int Month { get; set; }

        public string MonthName => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Month);
        public string Type { get; set; }
        public int Count { get; set; }

    }
}
