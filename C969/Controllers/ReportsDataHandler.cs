using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C969.Models;
using MySql.Data.MySqlClient;
using C969.Controllers;
using System.Windows.Forms;

namespace C969.Controllers
{
    /// <summary>
    /// Class that handles the data for the reports.
    /// </summary>
    public class ReportsDataHandler
    {
        private readonly MySqlConnection _connection;

        private readonly string _connString =
            ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;

        private readonly CustomerDataHandler _customerDataHandler;

        private TimeZoneInfo _userTimeZone = UserSession.CurrentTimeZone;


        public ReportsDataHandler(string currentUser)
        {
            _connection = new MySqlConnection(_connString);
            _customerDataHandler = new CustomerDataHandler(currentUser);
        }

        /// <summary>
        /// Method to get all users. Returns a list of users.
        /// </summary>
        /// <returns></returns>
        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            using (var conn = new MySqlConnection(_connString))
            {
                conn.Open();
                string query = "SELECT userId, userName FROM user";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new User
                            {
                                UserId = reader.GetInt32(0),
                                UserName = reader.GetString(1)
                            });
                        }
                    }
                }
            }

            return users;
        }

        /// <summary>
        /// Method to get appointments by user id. Returns all appointments for the user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<AppointmentDetails> GetAppointmentsByUserId(int userId)
        {
            List<AppointmentDetails> appointments = new List<AppointmentDetails>();
            TimeZoneInfo userTimeZone = UserSession.CurrentTimeZone;

            using (var conn = new MySqlConnection(_connString))
            {
                conn.Open();
                string query = @"
                SELECT appointmentId, customerId, userId, title, description, location, contact, type, url, start, end, createDate, createdBy, lastUpdate, lastUpdateBy
                FROM appointment
                WHERE userId = @userId";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            appointments.Add(_customerDataHandler.MapReaderToAppointmentDetails(reader, userTimeZone));
                        }
                    }
                }
            }

            return appointments;
        }

        /// <summary>
        /// Method to get the count of appointments by month and type.
        /// </summary>
        /// <returns></returns>
        public List<AppointmentTypesByMonths> GetAppointmentTypesByMonth()
        {
            List<AppointmentDetails> appointments = new List<AppointmentDetails>();
            using (var conn = new MySqlConnection(_connString))
            {
                conn.Open();
                string query = @"
                SELECT appointmentId, customerId, userId, title, description, location, contact, type, url, start, end, createDate, createdBy, lastUpdate, lastUpdateBy
                FROM appointment";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            appointments.Add(
                                _customerDataHandler.MapReaderToAppointmentDetails(reader,
                                    UserSession.CurrentTimeZone));
                        }
                    }
                }
            }

            //Groups appointments by month and type
            var groupedAppointments = appointments.GroupBy(app => new { app.Start.Month, AppointmentType = app.Type })
                .Select(group => new AppointmentTypesByMonths
                {
                    Month = group.Key.Month,
                    Type = group.Key.AppointmentType,
                    Count = group.Count()
                }).ToList();

            return groupedAppointments;

        }

        /// <summary>
        /// Method to get the count of appointments by user.
        /// </summary>
        /// <returns></returns>
        private List<Customer> GetAllCustomers()
        {
            List<Customer> customers = new List<Customer>();
            using (var conn = new MySqlConnection(_connString))
            {
                conn.Open();
                string query =
                    "SELECT customerId, customerName, addressId, active, createDate, createdBy, lastUpdate, lastUpdateBy FROM customer";


                using (var cmd = new MySqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            customers.Add(new Customer
                            (
                                reader.GetInt32("customerId"),
                                reader.GetString("customerName"),
                                reader.GetInt32("addressId"),
                                reader.GetInt32("active"),
                                reader.GetDateTime("createDate"),
                                reader.GetString("createdBy"),
                                reader.GetDateTime("lastUpdate"),
                                reader.GetString("lastUpdateBy")
                            ));
                        }
                    }
                }
            }

            return customers;
        }

        /// <summary>
        /// Method to get all cities. Returns a list of cities.
        /// </summary>
        /// <returns></returns>
        private List<City> GetAllCities()
        {
            List<City> cities = new List<City>();
            using (var conn = new MySqlConnection(_connString))
            {
                conn.Open();
                string query = "SELECT cityId, city, countryId FROM city";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cities.Add(new City
                            {
                                CityId = reader.GetInt32("cityId"),
                                CountryId = reader.GetInt32("countryId"),
                                CityName = reader.GetString("city")
                            });
                        }
                    }
                }
            }

            return cities;
        }

        /// <summary>
        /// Method to get all countries. Returns a list of countries.
        /// </summary>
        /// <returns></returns>
        public List<Country> GetAllCountries()
        {
            List<Country> countries = new List<Country>();
            using (var conn = new MySqlConnection(_connString))
            {
                conn.Open();
                string query = "SELECT countryId, country FROM country";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            countries.Add(new Country
                            {
                                CountryId = reader.GetInt32("countryId"),
                                CountryName = reader.GetString("country")
                            });
                        }
                    }
                }
            }

            return countries;
        }

     
        /// <summary>
        /// Get the Customer Name by its Id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public string GetCustomerNameById(int customerId)
        {
            var customer = _customerDataHandler.GetCustomerDetails(customerId);
            return customer != null ? customer.CustomerName : "Unknown";
        }


        public List<AppointmentCountByCustomer> GetAppointmentsCountByCustomer()
        {
            {
                List<AppointmentDetails> appointments = _customerDataHandler.GetAllAppointments();
                List<Customer> customers = GetAllCustomers();

                var appointmentsByCustomer = customers
                    .GroupJoin(
                        appointments,
                        customer => customer.CustomerID,
                        appointment => appointment.CustomerId,
                        (customer, appointmentGroup) => new AppointmentCountByCustomer
                        {
                            CustomerId = customer.CustomerID,
                            CustomerName = customer.CustomerName,
                            AppointmentsCount = appointmentGroup.Count()
                        }
                    )
                    .ToList();

                return appointmentsByCustomer;
            }

        }


    }
}
