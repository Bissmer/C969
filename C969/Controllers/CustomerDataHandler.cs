using System;
using System.Collections;
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
        private readonly MySqlConnection _connection;
        private readonly string _connString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
        private readonly string _currentUser;


        public CustomerDataHandler(string _connString)
        {
            _connection = new MySqlConnection(_connString);
            _currentUser = UserSession.CurrentUser;
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
                cmd.Parameters.AddWithValue("@now", DateTime.UtcNow);
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
                cmd.Parameters.AddWithValue("@now", DateTime.UtcNow);
                cmd.Parameters.AddWithValue("@user", _currentUser);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Retrieves the details of a customer from the database.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Updates the details of a customer in the database.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public bool UpdateCustomerDetails(CustomerDetails customer)
        {
            using (var conn = new MySqlConnection(_connString))
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        // Update or add the country first to get the countryId
                        int countryId = EnsureCountry(customer.Country, conn, trans);
                        // Update the city first to get the cityId
                        int cityId = EnsureCity(customer.City, countryId, conn, trans);

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
                        Console.WriteLine($"Updating Customer ID: {customer.CustomerID}");
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

        /// <summary>
        /// Ensures that the given city exists in the database. If it does not exist, it is inserted and the new city ID is returned.
        /// </summary>
        /// <param name="cityName"></param>
        /// <param name="countryId"></param>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        private int EnsureCity(string cityName, int countryId, MySqlConnection conn, MySqlTransaction trans)
        {
            int cityId = GetCityId(cityName, countryId, conn, trans);
            if (cityId == 0)
            {
                string insertCity = @"
                    INSERT INTO City (city, countryId, createDate, createdBy, lastUpdate, lastUpdateBy)
                    VALUES (@cityName, @countryId, NOW(), @createdBy, NOW(), @lastUpdateBy);
                    SELECT LAST_INSERT_ID();";

                using (var cmd = new MySqlCommand(insertCity, conn, trans))
                {
                    cmd.Parameters.AddWithValue("@cityName", cityName); 
                    cmd.Parameters.AddWithValue("@countryId", countryId);
                    cmd.Parameters.AddWithValue("@createDate", DateTime.UtcNow);
                    cmd.Parameters.AddWithValue("@createdBy", _currentUser);
                    cmd.Parameters.AddWithValue("@lastUpdateBy", _currentUser);
                    cityId = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            return cityId;
        }

        /// <summary>
        /// Retrieves the city ID for the given city name and country ID.
        /// </summary>
        /// <param name="cityName"></param>
        /// <param name="countryId"></param>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        private int GetCityId(string cityName, int countryId, MySqlConnection conn, MySqlTransaction trans)
        {
            string query = "SELECT cityId FROM City WHERE city = @cityName AND countryId = @countryId;";
            using (var cmd = new MySqlCommand(query, conn, trans))
            {
                cmd.Parameters.AddWithValue("@cityName", cityName);
                cmd.Parameters.AddWithValue("@countryId", countryId);
                var result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        /// <summary>
        /// Ensures that the given country exists in the database. If it does not exist, it is inserted and the new country ID is returned.
        /// </summary>
        /// <param name="countryName"></param>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Retrieves the country ID for the given country name.
        /// </summary>
        /// <param name="countryName"></param>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public int GetCountryId(string countryName, MySqlConnection conn, MySqlTransaction trans)
        {
            string query = "SELECT countryId FROM Country WHERE country = @countryName;";
            using (var cmd = new MySqlCommand(query, conn, trans))
            {
                cmd.Parameters.AddWithValue("@countryName", countryName);
                var result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        /// <summary>
        /// Retrieves a list of all countries from the database.
        /// </summary>
        /// <returns></returns>
        public List<string> GetCountries()
        {
            List<string> countries = new List<string>();
            using (var conn = new MySqlConnection(_connString))
            {
                conn.Open();
                string query = "SELECT country FROM Country ORDER BY country;";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            countries.Add(reader["country"].ToString());
                        }
                    }
                }
            }

            return countries;
        }

        /// <summary>
        /// Retrieves all customers from the database.
        /// </summary>
        /// <returns></returns>
        public List<CustomerDetails> GetAllCustomers()
        {
            List<CustomerDetails> customers = new List<CustomerDetails>();
            using (MySqlConnection conn = new MySqlConnection(_connString))
            {
                conn.Open();
                string query = @"
                SELECT c.customerId, c.customerName, a.address, a.address2, a.phone, ct.city, a.postalCode, co.country, c.active
                FROM Customer c
                JOIN Address a ON c.addressId = a.addressId
                JOIN City ct ON a.cityId = ct.cityId
                JOIN Country co ON ct.countryId = co.countryId";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            customers.Add(new CustomerDetails
                            {
                                CustomerID = reader.GetInt32("customerId"),
                                CustomerName = reader.GetString("customerName"),
                                Address = reader.GetString("address"),
                                Address2 = reader.IsDBNull(reader.GetOrdinal("address2")) ? null : reader.GetString("address2"),
                                Phone = reader.GetString("phone"),
                                PostalCode = reader.GetString("postalCode"),
                                City = reader.GetString("city"),
                                Country = reader.GetString("country"),
                                IsActive = reader.GetBoolean("active")
                            });
                        }
                    }
                }
            }
            return customers;
        }

        /// <summary>
        /// Deletes a customer from the database.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public bool DeleteCustomer(int customerId)
        {

            if (CustomerHasAppointments(customerId))
            {
                throw new InvalidOperationException("Customer has related appointments. Please delete the appointments first.");
            }

            using (var conn = new MySqlConnection(_connString))
            {
                conn.Open();
                var query = "DELETE FROM Customer WHERE CustomerID = @customerId";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@customerId", customerId);
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
        }

        /// <summary>
        /// Retrieves all appointments from the database.
        /// </summary>
        /// <returns></returns>
        public List<AppointmentDetails> GetAllAppointments()
        {
            List<AppointmentDetails> appointments = new List<AppointmentDetails>();
            TimeZoneInfo userTimeZone = UserSession.CurrentTimeZone;
            TimeZoneInfo estTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

            using (var conn = new MySqlConnection(_connString))
            {
                conn.Open();
                string query = @"
                SELECT appointmentId, customerId, userId, title, description, location, contact, type, url,
                   start, end, createDate, createdBy, lastUpdate, lastUpdateBy
                FROM appointment";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Read the start and end times from the database and specify the DateTimeKind
                            DateTime estStart = DateTime.SpecifyKind(reader.GetDateTime("start"), DateTimeKind.Unspecified);
                            DateTime estEnd = DateTime.SpecifyKind(reader.GetDateTime("end"), DateTimeKind.Unspecified);
                            DateTime estCreate = DateTime.SpecifyKind(reader.GetDateTime("createDate"), DateTimeKind.Unspecified);
                            DateTime estLastUpdate = DateTime.SpecifyKind(reader.GetDateTime("lastUpdate"), DateTimeKind.Unspecified);


                            // Convert the start and end times from EST to the user's local time zone
                            DateTime localStart = TimeZoneInfo.ConvertTime(estStart, estTimeZone, userTimeZone);
                            DateTime localEnd = TimeZoneInfo.ConvertTime(estEnd, estTimeZone, userTimeZone);
                            DateTime localCreateDate = TimeZoneInfo.ConvertTime(estCreate, userTimeZone);
                            DateTime localLastUpdate = TimeZoneInfo.ConvertTime(estLastUpdate,userTimeZone);



                            appointments.Add(new AppointmentDetails
                            {
                                AppointmentId = reader.GetInt32("appointmentId"),
                                CustomerId = reader.GetInt32("customerId"),
                                UserId = reader.GetInt32("userId"),
                                Title = reader.GetString("title"),
                                Description = reader.GetString("description"),
                                Location = reader.GetString("location"),
                                Contact = reader.GetString("contact"),
                                Type = reader.GetString("type"),
                                Url = reader.GetString("url"),
                                Start = localStart,
                                End = localEnd,
                                CreateDate = localCreateDate,
                                CreatedBy = reader.GetString("createdBy"),
                                LastUpdate = localLastUpdate,
                                LastUpdateBy = reader.GetString("lastUpdateBy")
                            });
                        }
                    }
                }
            }
            return appointments;
        }

        /// <summary>
        /// Retrieves a list of all customer names with their IDs.
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetCustomerNameAndId()
        {
            var customers = new Dictionary<int, string>();
            using (var conn = new MySqlConnection(_connString))
            {
                conn.Open();
                string query = "SELECT customerId, customerName FROM Customer";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int customerId = reader.GetInt32("customerId");
                            string customerName = reader.GetString("customerName");
                            customers.Add(customerId, customerName);
                        }
                    }
                }

            }
            return customers;
        }

        /// <summary>
        /// Adds an appointment to the database.
        /// </summary>
        /// <param name="appointment"></param>
        /// <returns></returns>
        public bool AddAppointment(AppointmentDetails appointment)
        {
            var (estStart, estEnd) =
                TimeZoneHandler.ConvertToEst(appointment.Start, appointment.End, UserSession.CurrentTimeZone);

            // Convert the current time to UTC for CreateDate
            DateTime localNow = TimeZoneInfo.ConvertTime(DateTime.UtcNow, UserSession.CurrentTimeZone);

            using (var conn = new MySqlConnection(_connString))
            {
                conn.Open();
                var query = @"
                  INSERT INTO appointment (CustomerId, UserId, Title, Description, Location, Contact, Type, Url, Start, End, CreatedBy,CreateDate, LastUpdateBy )
                   VALUES (@CustomerId, @UserId, @Title, @Description, @Location, @Contact, @Type, @Url, @Start, @End, @CreatedBy,@CreateDate,@LastUpdateBy );";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CustomerId", appointment.CustomerId);
                    cmd.Parameters.AddWithValue("@UserId", appointment.UserId);
                    cmd.Parameters.AddWithValue("@Title", appointment.Title);
                    cmd.Parameters.AddWithValue("@Description", appointment.Description);
                    cmd.Parameters.AddWithValue("@Location", appointment.Location);
                    cmd.Parameters.AddWithValue("@Contact", appointment.Contact);
                    cmd.Parameters.AddWithValue("@Type", appointment.Type);
                    cmd.Parameters.AddWithValue("@Url", appointment.Url);
                    cmd.Parameters.AddWithValue("@Start", estStart);
                    cmd.Parameters.AddWithValue("@End", estEnd);
                    cmd.Parameters.AddWithValue("@CreatedBy", appointment.CreatedBy);
                    cmd.Parameters.AddWithValue("@CreateDate", localNow);
                    cmd.Parameters.AddWithValue("@LastUpdateBy", UserSession.CurrentUser);
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
        }

        public List<AppointmentDetails> GetAppointmentsByCustomerName(string customerName)
        {
            List<AppointmentDetails> filteredAppointments = new List<AppointmentDetails>();
            TimeZoneInfo userTimeZone = UserSession.CurrentTimeZone;

            using (var conn = new MySqlConnection(_connString))
            {
                conn.Open();
                string query = @"
                SELECT a.* FROM appointment a
                JOIN customer c ON a.customerId = c.customerId
                WHERE c.customerName LIKE @customerName";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@customerName", "%" + customerName + "%");
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            filteredAppointments.Add(MapReaderToAppointmentDetails(reader, userTimeZone));
                        }
                    }
                }
            }
            return filteredAppointments;
        }

        /// <summary>
        /// Maps the data from a MySqlDataReader to an AppointmentDetails object.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public AppointmentDetails MapReaderToAppointmentDetails(MySqlDataReader reader, TimeZoneInfo userTimeZone )
        {

            // Get the time zone for Eastern Standard Time
            TimeZoneInfo estTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

            // Read the start and end times from the database as EST
            DateTime estStart = reader.GetDateTime("start");
            DateTime estEnd = reader.GetDateTime("end");

            // Convert the start and end times from EST to the user's local time zone
            DateTime localStart = TimeZoneInfo.ConvertTime(estStart, estTimeZone, userTimeZone);
            DateTime localEnd = TimeZoneInfo.ConvertTime(estEnd, estTimeZone, userTimeZone);

            DateTime localCreateDate = reader.GetDateTime("createDate");
            DateTime localLastUpdate = reader.GetDateTime("lastUpdate");

            return new AppointmentDetails
            {
                AppointmentId = reader.GetInt32("appointmentId"),
                CustomerId = reader.GetInt32("customerId"),
                UserId = reader.GetInt32("userId"),
                Title = reader["title"].ToString(),
                Description = reader["description"].ToString(),
                Location = reader["location"].ToString(),
                Contact = reader["contact"].ToString(),
                Type = reader["type"].ToString(),
                Url = reader.IsDBNull(reader.GetOrdinal("url")) ? null : reader["url"].ToString(), // Handling nullable fields
                Start = localStart,
                End = localEnd,
                CreateDate = localCreateDate,
                CreatedBy = reader["createdBy"].ToString(),
                LastUpdate = localLastUpdate,
                LastUpdateBy = reader["lastUpdateBy"].ToString()
            };
        }

        /// <summary>
        /// Retrieves an appointment by its ID.
        /// </summary>
        /// <param name="appointmentId"></param>
        /// <returns></returns>
        public AppointmentDetails GetAppointmentById(int appointmentId)
        {
            TimeZoneInfo userTimeZone = UserSession.CurrentTimeZone;

            using (var conn = new MySqlConnection(_connString))
            {
                conn.Open(); 
                string query = "SELECT * FROM appointment WHERE appointmentId = @appointmentId";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@appointmentId", appointmentId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapReaderToAppointmentDetails(reader, userTimeZone);
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Function to update an appointment in the database.
        /// </summary>
        /// <param name="appointment"></param>
        /// <returns></returns>
        public bool UpdateAppointment(AppointmentDetails appointment)
        {
            // Convert the appointment start and end times to EST
            var (estStart, estEnd) =
                TimeZoneHandler.ConvertToEst(appointment.Start, appointment.End, UserSession.CurrentTimeZone);
            
            // Convert the current time to UTC for CreateDate
            DateTime localNow = TimeZoneInfo.ConvertTime(DateTime.UtcNow, UserSession.CurrentTimeZone);

            using (var conn = new MySqlConnection(_connString))
            {
                conn.Open();

                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        string query = @"
                        UPDATE appointment
                        SET
                        customerId = @customerId, userId = @userId, title = @title, description = @description, location = @location,
                        contact = @contact, type = @type, url = @url, start = @start, end = @end, lastUpdate = @lastUpdate,lastUpdateBy = @lastUpdateBy
                        WHERE appointmentId = @appointmentId;";


                        using (var cmd = new MySqlCommand(query, conn, trans))
                        {
                            cmd.Parameters.AddWithValue("@customerId", appointment.CustomerId);
                            cmd.Parameters.AddWithValue("@userId", appointment.UserId);
                            cmd.Parameters.AddWithValue("@title", appointment.Title);
                            cmd.Parameters.AddWithValue("@description", appointment.Description);
                            cmd.Parameters.AddWithValue("@location", appointment.Location);
                            cmd.Parameters.AddWithValue("@contact", appointment.Contact);
                            cmd.Parameters.AddWithValue("@type", appointment.Type);
                            cmd.Parameters.AddWithValue("@url", appointment.Url);
                            cmd.Parameters.AddWithValue("@start", estStart);
                            cmd.Parameters.AddWithValue("@end", estEnd);
                            cmd.Parameters.AddWithValue("@lastUpdate", localNow);
                            cmd.Parameters.AddWithValue("@lastUpdateBy", UserSession.CurrentUser);
                            cmd.Parameters.AddWithValue("@appointmentId", appointment.AppointmentId);

                            Console.WriteLine($"Updating appointment with ID: {appointment.AppointmentId}");
                            int result = cmd.ExecuteNonQuery();
                            trans.Commit();
                            return result > 0;
                        }
                    }
                    catch (MySqlException ex)
                    {
                        trans.Rollback();
                        Console.WriteLine($"An error occurred: {ex.Message}");
                        return false;
                    }
                    finally
                    {
                        if (conn.State == System.Data.ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                    
                }
                    
            }

            
        }

        /// <summary>
        /// Function to delete an appointment from the database.
        /// </summary>
        /// <param name="appointmentId"></param>
        /// <returns></returns>
        public bool DeleteAppointment(int appointmentId)
        {
            using (var conn = new MySqlConnection(_connString))
            {
                conn.Open();
                string query = "DELETE FROM appointment WHERE appointmentId = @appointmentId";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@appointmentId", appointmentId);
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
        }

        /// <summary>
        /// Function to retrieve appointments by date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<AppointmentDetails> GetAppointmentsByDate(DateTime date)
        {
            List<AppointmentDetails> appointments = new List<AppointmentDetails>();
            TimeZoneInfo userTimeZone = UserSession.CurrentTimeZone;

            // Convert the local date to the start and end of the day in UTC
            DateTime startOfLocalDay = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Local);
            DateTime endOfLocalDay = startOfLocalDay.AddDays(1).AddTicks(-1);

            DateTime utcStart = TimeZoneInfo.ConvertTimeToUtc(startOfLocalDay, userTimeZone);
            DateTime utcEnd = TimeZoneInfo.ConvertTimeToUtc(endOfLocalDay, userTimeZone);

            using (var conn = new MySqlConnection(_connString))
            {
                conn.Open();
                string query = @"
                SELECT appointmentId, customerId, userId, title, description, location, contact, type, url, start, end, createDate, createdBy, lastUpdate, lastUpdateBy
                FROM appointment
                WHERE start >= @UtcStart AND start <= @UtcEnd"; 

                using (var cmd = new MySqlCommand(query, conn))
                {

                    cmd.Parameters.AddWithValue("@UtcStart", utcStart);
                    cmd.Parameters.AddWithValue("@UtcEnd", utcEnd);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            appointments.Add(MapReaderToAppointmentDetails(reader, userTimeZone));
                        }
                    }
                }
            }
            return appointments;
        }
        /// <summary>
        /// Method to retrieve a list of dates with appointments.
        /// </summary>
        /// <returns></returns>
        public List<DateTime> GetDatesWithAppointments()
        {
            List<DateTime> dates = new List<DateTime>();
            using (var conn = new MySqlConnection(_connString))
            {
                conn.Open();
                string query = "SELECT DISTINCT DATE(start) AS AppointmentDate FROM appointment";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dates.Add(reader.GetDateTime("AppointmentDate"));
                        }
                    }
                }
            }
            return dates;
        }

        /// <summary>
        /// Method to check if a customer has appointments.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public bool CustomerHasAppointments(int customerId)
        {
            using (var conn = new MySqlConnection(_connString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM appointment WHERE customerId = @customerId";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@customerId", customerId);
                    int result = Convert.ToInt32(cmd.ExecuteScalar());
                    return result > 0;
                }
            }
        }

        /// <summary>
        /// Method to retrieve upcoming appointments for a user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<AppointmentDetails> GetUpcomingAppointments( int userId)
        {
            List<AppointmentDetails> upcomingAppointments = new List<AppointmentDetails>();
            DateTime utcNow = DateTime.UtcNow;
            DateTime utcFuture = utcNow.AddMinutes(15); // 15 minutes from now

            using (var conn = new MySqlConnection(_connString))
            {
                conn.Open();
                string query = @"
                SELECT appointmentId, customerId, userId, title, description, location, contact, type, url, start, end, createDate, createdBy, lastUpdate, lastUpdateBy
                FROM appointment
                WHERE userId = @userId AND start BETWEEN @utcNow AND @utcFuture";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@utcNow", utcNow);
                    cmd.Parameters.AddWithValue("@utcFuture", utcFuture);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            upcomingAppointments.Add(MapReaderToAppointmentDetails(reader, UserSession.CurrentTimeZone));
                        }
                    }
                }
            }
            return upcomingAppointments;
        }

    }
}
