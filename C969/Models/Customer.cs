﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C969.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public int AddressID { get; set; }
        public int Active { get; set; }
        private DateTime createDate;
        public DateTime CreateDate { 
            get => createDate; 
            set => createDate = value.Date; } //Truncate the time portion of the date
        public string CreatedBy { get; set; }
        public DateTime LastUpdate { get; set; }
        public string LastUpdateBy { get; set; }


        //Constructor to create  a Customer w/o an ID

        public Customer(string customerName, int addressID, int active, string createdBy, string lastUpdateBy)
        {
            CustomerName = customerName.Trim();
            AddressID = addressID;
            Active = active;
            CreateDate = DateTime.Now; //assuming that the CreateDate is the current date and time
            CreatedBy = createdBy;
            LastUpdate = DateTime.Now; // Assuming last update is set to the current date/time for new records
            LastUpdateBy = lastUpdateBy;
        }

        // Constructor for existing customer records where CustomerID is known

        public Customer(int customerID, string customerName, int addressID, int active, DateTime createDate, string createdBy, DateTime lastUpdate,
            string lastUpdateBy)
        {
            CustomerID = customerID;
            CustomerName = customerName.Trim();
            AddressID = addressID;
            Active = active;
            CreateDate = createDate;
            CreatedBy = createdBy.Trim();
            LastUpdate = lastUpdate;
            LastUpdateBy = lastUpdateBy.Trim();
        }

    }
}
