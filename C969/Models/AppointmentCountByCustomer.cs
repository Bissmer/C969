using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C969.Models
{
    public class AppointmentCountByCustomer
    {

        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int AppointmentsCount { get; set; }
    }
}
