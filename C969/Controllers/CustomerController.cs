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
        private string _connString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;

        // Add a new customer to the database
        public void AddCustomer(Customer customer)
        {
            using (MySqlConnection connection = new MySqlConnection(_connString))
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
            using (MySqlConnection connection = new MySqlConnection(_connString))
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
            using (MySqlConnection connection = new MySqlConnection(_connString))
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

            using (MySqlConnection connection = new MySqlConnection(_connString))
            {
                string query = "SELECT * FROM customer WHERE customerID = @customerID";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@customerID", customerID);

                    connection.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            customer = new Customer(

                                Convert.ToInt32(reader["customerID"]),
                                reader["customerName"].ToString(),
                                Convert.ToInt32(reader["addressID"]),
                                Convert.ToInt32(reader["active"]),
                                Convert.ToDateTime(reader["createDate"]),
                                reader["createdBy"].ToString(),
                                Convert.ToDateTime(reader["lastUpdate"]),
                                reader["lastUpdateBy"].ToString()
                                );
                        }
                    }
                }
            }

            return customer;
        }
        // Retrieve all customers from the database
        public List<Customer> GetAllCustomers()
        {
            List<Customer> customers = new List<Customer>();

            using (MySqlConnection connection = new MySqlConnection(_connString))
            {
                string query = "SELECT * FROM customer";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Customer customer = new Customer(
                                Convert.ToInt32(reader["customerID"]),
                                reader["customerName"].ToString(),
                                Convert.ToInt32(reader["addressID"]),
                                Convert.ToInt32(reader["active"]),
                                Convert.ToDateTime(reader["createDate"]),
                                reader["createdBy"].ToString(),
                                Convert.ToDateTime(reader["lastUpdate"]),
                                reader["lastUpdateBy"].ToString()
                                );
                            customers.Add(customer);
                        }
                    }
                }

            }
            return customers;
        }

        public bool AddressExists(int addressID)
        {
            using (MySqlConnection connection = new MySqlConnection(_connString))
            {
                string query = "SELECT COUNT(1) FROM Address WHERE AddressID = @AddressID";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@AddressID", addressID);

                connection.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }
    }

}

