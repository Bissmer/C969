using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C969.Models
{

    /// <summary>
    /// Class is used to store the count of appointments by customer
    /// </summary>
    public class AppointmentCountByCustomer
    {

        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int AppointmentsCount { get; set; }
    }
}
