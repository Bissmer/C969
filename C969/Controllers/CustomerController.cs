using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C969.Models;
using MySql.Data.MySqlClient;

namespace C969.Controllers
{
    public class CustomerController
    {
        private string connString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;

        // Add a new customer to the database
        public void AddCustomer(Customer customer)
        {
            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                string query = @"INSERT INTO customer (customerName, addressID, active, createDate, createdBy, lastUpdate, lastUpdateBy)
                                VALUES (@customerName, @addressID, @active, @createDate, @createdBy, @lastUpdate, @lastUpdateBy)";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@customerName", customer.CustomerName);
                    cmd.Parameters.AddWithValue("@addressID", customer.AddressID);
                    cmd.Parameters.AddWithValue("@active", customer.Active);
                    cmd.Parameters.AddWithValue("@createDate", customer.CreateDate);
                    cmd.Parameters.AddWithValue("@createdBy", customer.CreatedBy);
                    cmd.Parameters.AddWithValue("@lastUpdate", customer.LastUpdate);
                    cmd.Parameters.AddWithValue("@lastUpdateBy", customer.LastUpdateBy);

                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                }
            }
        }

        // Update an existing customer in the database

        public void UpdateCustomer(Customer customer)
        {
            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                string query = @"UPDATE customer
                                SET customerName = @customerName, addressID = @addressID, active = @active, createDate = @createDate, createdBy = @createdBy, lastUpdate = @lastUpdate, lastUpdateBy = @lastUpdateBy
                                WHERE customerID = @customerID";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@customerID", customer.CustomerID);
                    cmd.Parameters.AddWithValue("@customerName", customer.CustomerName);
                    cmd.Parameters.AddWithValue("@addressID", customer.AddressID);
                    cmd.Parameters.AddWithValue("@active", customer.Active);
                    cmd.Parameters.AddWithValue("@createDate", customer.CreateDate);
                    cmd.Parameters.AddWithValue("@createdBy", customer.CreatedBy);
                    cmd.Parameters.AddWithValue("@lastUpdate", customer.LastUpdate);
                    cmd.Parameters.AddWithValue("@lastUpdateBy", customer.LastUpdateBy);

                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }

            }
        }

        // Delete a customer from the database

        public void DeleteCustomer (int customerID)
        {
            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                string query = "DELETE FROM customer WHERE customerID = @customerID";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@customerID", customerID);

                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        //Retrieve a customer from the database by ID
        public Customer GetCustomerByID(int customerID)
        {
            Customer customer = null;

            using (MySqlConnection connection = new MySqlConnection(connString))
            {

            }


        }
    }

}

