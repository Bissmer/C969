using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using C969.Models;
using MySql.Data.MySqlClient;

namespace C969.Controllers
{
    public class CustomerDataHandler
    {
        private MySqlConnection _connection;
        private string _connString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
        private string _currentUser = UserSession.CurrentUser;


        public CustomerDataHandler(string connString, string currentUser)
        {
            _connection = new MySqlConnection(_connString);
            _currentUser = currentUser;
        }

        /// <summary>
        /// Adds a customer to the database with the given details.
        /// </summary>
        /// <param name="customerName"></param>
        /// <param name="address"></param>
        /// <param name="address2"></param>
        /// <param name="phone"></param>
        /// <param name="city"></param>
        /// <param name="postalCode"></param>
        /// <param name="country"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public bool AddCustomerWithDetails(string customerName, string address, string address2,
            string phone, string city, string postalCode, string country, bool isActive)
        {
            MySqlTransaction transaction = null;

            try
            {
                _connection.Open();
                transaction = _connection.BeginTransaction();

                var countryId = EnsureEntity("Country", transaction, new MySqlParameter("@country", country));
                var cityId = EnsureEntity("City", transaction, new MySqlParameter("@city", city), new MySqlParameter("@countryId", countryId));
                var addressId = EnsureEntity("Address", transaction,
                    new MySqlParameter("@address", address),
                    new MySqlParameter("@address2", address2),
                    new MySqlParameter("@cityId", cityId),
                    new MySqlParameter("@postalCode", postalCode),
                    new MySqlParameter("@phone", phone));

                InsertCustomer(customerName, addressId, isActive, transaction);

                transaction.Commit();
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                transaction?.Rollback();
                return false;
            }
            finally
            {
                {
                    _connection.Close();
                }

            }
        }

        /// <summary>
        /// Searches for an entity in the database and returns its ID if found. If not found, inserts the entity and returns the new ID.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="transaction"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int EnsureEntity(string tableName, MySqlTransaction transaction, params MySqlParameter[] parameters)
        {

            string whereConditions = string.Join(" AND ", Array.ConvertAll(parameters, p => $"{p.ParameterName.Substring(1)} = {p.ParameterName}"));
            string selectQuery = $"SELECT {tableName}Id FROM {tableName} WHERE {whereConditions}";

            using (var cmd = new MySqlCommand(selectQuery, _connection, transaction))
            {
                cmd.Parameters.AddRange(parameters);
                var result = cmd.ExecuteScalar();

                // Clear parameters after executing the select command to avoid "parameter already defined" error.
                cmd.Parameters.Clear();

                if (result != null)
                    return Convert.ToInt32(result);

                string insertFields = string.Join(", ", Array.ConvertAll(parameters, p => p.ParameterName.Substring(1)));
                string insertValues = string.Join(", ", Array.ConvertAll(parameters, p => p.ParameterName));
                string insertQuery = $@"
            INSERT INTO {tableName} ({insertFields}, createDate, createdBy, lastUpdate, lastUpdateBy)
            VALUES ({insertValues}, @now, @user, @now, @user); 
            SELECT LAST_INSERT_ID();";

                cmd.CommandText = insertQuery;
                cmd.Parameters.AddRange(parameters);  // Re-add parameters for the insert operation
                cmd.Parameters.AddWithValue("@now", DateTime.Now);
                cmd.Parameters.AddWithValue("@user", _currentUser);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }

        }

        /// <summary>
        /// Inserts a new customer into the database.
        /// </summary>
        /// <param name="customerName"></param>
        /// <param name="addressId"></param>
        /// <param name="isActive"></param>
        /// <param name="transaction"></param>
        public void InsertCustomer(string customerName, int addressId, bool isActive, MySqlTransaction transaction)
        {
            string insertQuery = @"
            INSERT INTO Customer (customerName, addressId, active, createDate, createdBy, lastUpdate, lastUpdateBy) 
            VALUES (@customerName, @addressId, @active, @now, @user, @now, @user)";

            using (var cmd = new MySqlCommand(insertQuery, _connection, transaction))
            {
                cmd.Parameters.AddWithValue("@customerName", customerName);
                cmd.Parameters.AddWithValue("@addressId", addressId);
                cmd.Parameters.AddWithValue("@active", isActive ? 1 : 0);
                cmd.Parameters.AddWithValue("@now", DateTime.Now);
                cmd.Parameters.AddWithValue("@user", _currentUser);
                cmd.ExecuteNonQuery();
            }
        }

        public CustomerDetails GetCustomerDetails(int customerId)
        {
            CustomerDetails details = null;

            string query = @"
            SELECT c.customerId, c.customerName, a.address, a.address2, a.phone, ct.city, a.postalCode, co.country, c.active
            FROM Customer c
            JOIN Address a ON c.addressId = a.addressId
            JOIN City ct ON a.cityId = ct.cityId
            JOIN Country co ON ct.countryId = co.countryId
            WHERE c.customerId = @customerId";

            using (var conn = new MySqlConnection(_connString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@customerId", customerId);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            details = new CustomerDetails
                            {
                                CustomerID = reader.GetInt32("customerId"),
                                CustomerName = reader["customerName"].ToString(),
                                Address = reader["address"].ToString(),
                                Address2 = reader["address2"].ToString(),
                                Phone = reader["phone"].ToString(),
                                City = reader["city"].ToString(),
                                PostalCode = reader["postalCode"].ToString(),
                                Country = reader["country"].ToString(),
                                IsActive = reader.GetBoolean("active")
                            };
                        }
                    }
                }
            }
            return details;
        }

        public bool UpdateCustomerDetails(CustomerDetails customer)
        {
            using (var conn = new MySqlConnection(_connString))
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        // Update the city first to get the cityId
                        int cityId = EnsureCity(customer.City, customer.Country, conn, trans);

                        // Update the Address
                        string updateAddress = @"
                        UPDATE Address
                        SET address = @address, address2 = @address2, phone = @phone, postalCode = @postalCode, cityId = @cityId
                        WHERE addressId = (SELECT addressId FROM Customer WHERE customerId = @customerId);";
                        using (var cmd = new MySqlCommand(updateAddress, conn, trans))
                        {
                            cmd.Parameters.AddWithValue("@address", customer.Address);
                            cmd.Parameters.AddWithValue("@address2", customer.Address2);
                            cmd.Parameters.AddWithValue("@phone", customer.Phone);
                            cmd.Parameters.AddWithValue("@postalCode", customer.PostalCode);
                            cmd.Parameters.AddWithValue("@cityId", cityId);
                            cmd.Parameters.AddWithValue("@customerId", customer.CustomerID);
                            cmd.ExecuteNonQuery();
                        }

                        // Update the Customer record
                        string updateCustomer = @"
                        UPDATE Customer
                        SET customerName = @customerName, active = @active
                        WHERE customerId = @customerId;";
                        using (var cmd = new MySqlCommand(updateCustomer, conn, trans))
                        {
                            cmd.Parameters.AddWithValue("@customerName", customer.CustomerName);
                            cmd.Parameters.AddWithValue("@active", customer.IsActive);
                            cmd.Parameters.AddWithValue("@customerId", customer.CustomerID);
                            cmd.ExecuteNonQuery();
                        }

                        trans.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        Console.WriteLine($"An error occurred: {ex.Message}");
                        return false;
                    }
                }
            }
        }

        private int EnsureCity(string cityName, string countryName, MySqlConnection conn, MySqlTransaction trans)
        {
            int cityId = GetCityId(cityName, countryName, conn, trans);
            if (cityId == 0)
            {
                string insertCity =
                    "INSERT INTO City (city, countryId) VALUES (@cityName, (SELECT countryId FROM Country WHERE country = @countryName)); SELECT LAST_INSERT_ID();";
                using (var cmd = new MySqlCommand(insertCity, conn, trans))
                {
                    cmd.Parameters.AddWithValue("@cityName", cityName); 
                    cmd.Parameters.AddWithValue("@countryName", countryName);
                    cityId = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            return cityId;
        }

        private int GetCityId(string cityName, string countryName, MySqlConnection conn, MySqlTransaction trans)
        {
            string query = "SELECT cityId FROM City INNER JOIN Country ON City.countryId = Country.countryId WHERE City.city = @cityName AND Country.country = @countryName;";
            using (var cmd = new MySqlCommand(query, conn, trans))
            {
                cmd.Parameters.AddWithValue("@cityName", cityName);
                cmd.Parameters.AddWithValue("@countryName", countryName);
                var result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        private int EnsureCountry(string countryName, MySqlConnection conn, MySqlTransaction trans)
        {
            int countryId = GetCountryId(countryName, conn, trans);
            if (countryId == 0)
            {
                string insertCountry = "INSERT INTO Country (country) VALUES (@countryName); SELECT LAST_INSERT_ID();";
                using (var cmd = new MySqlCommand(insertCountry, conn, trans))
                {
                    cmd.Parameters.AddWithValue("@countryName", countryName);
                    countryId = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            return countryId;
        }

    }
}
