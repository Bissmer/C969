using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
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
    }
}
