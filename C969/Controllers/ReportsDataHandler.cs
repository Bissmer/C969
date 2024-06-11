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
        /// Method to get the count of customers by city. Returns a list of CustomerCount.
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public List<CustomerCount> GetCustomerCountByCity(int countryId)
        {

            List<Customer> customers = GetAllCustomers();
            List<City> cities = GetAllCities();

            // Get the count of customers by city
            var result = cities
                .Where(city => city.CountryId == countryId)
                .GroupJoin( // Join cities with customers` address
                    customers,
                    city => city.CityId,
                    customer => customer.AddressID,
                    (city, customerGroup) => new CustomerCount
                    {
                        Name = city.CityName,
                        CusCount = customerGroup.Count()
                    }
                ).ToList();
            return result;
        }

        /// <summary>
        /// Method to get the count of customers by country. Returns a list of CustomerCount.
        /// <returns></returns>
        public List<CustomerCount> GetCustomerCountByCountry()
        {
            List<Customer> customers = GetAllCustomers();
            List<City> cities = GetAllCities();
            List<Country> countries = GetAllCountries();


            if (customers == null || cities == null || countries == null)
            {
                throw new InvalidOperationException("Unable to load necessary data for customer count computation.");
            }

            // Step 1: Group customers by city
            var customerGroupsByCity = customers.GroupBy(c => c.AddressID)
                .Select(g => new
                {
                    CityId = g.Key,
                    CustomerCount = g.Count()
                }).ToList();

            Console.WriteLine("Customer Groups by City:");
            foreach (var group in customerGroupsByCity)
            {
                Console.WriteLine($"CityId: {group.CityId}, CustomerCount: {group.CustomerCount}");
            }

            // Step 2: Join cities with their customer counts
            var cityCustomerCounts = cities
                .GroupJoin(
                    customerGroupsByCity,
                    city => city.CityId,
                    customerGroup => customerGroup.CityId,
                    (city, customerGroup) => new
                    {
                        City = city,
                        CustomerCount = customerGroup.FirstOrDefault()?.CustomerCount ?? 0
                    }).ToList();

            Console.WriteLine("City Customer Counts:");
            foreach (var count in cityCustomerCounts)
            {
                Console.WriteLine($"City: {count.City.CityName}, CustomerCount: {count.CustomerCount}");
            }

            // Step 3: Group by country and sum the customer counts
            var countryCustomerCounts = countries
                .GroupJoin(
                    cityCustomerCounts,
                    country => country.CountryId,
                    cityCustomerCount => cityCustomerCount.City.CountryId,
                    (country, cityGroup) => new
                    {
                        Country = country,
                        CustomerCount = cityGroup.Sum(c => c.CustomerCount)
                    })
                .Select(g => new CustomerCount
                {
                    Name = g.Country.CountryName,
                    CusCount = g.CustomerCount
                }).ToList();

            Console.WriteLine("Country Customer Counts:");
            foreach (var count in countryCustomerCounts)
            {
                Console.WriteLine($"Country: {count.Name}, CustomerCount: {count.CusCount}");
            }


            return countryCustomerCounts;
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

        public List<AppointmentCountByCountry> GetAppointmentCountByCountry()
        {
            List<AppointmentDetails> appointments = _customerDataHandler.GetAllAppointments();
            List<City> cities = GetAllCities();
            List<Country> countries = GetAllCountries();

            var output = countries
                .GroupJoin(
                    cities,
                    country => country.CountryId,
                    city => city.CountryId,
                    (country, cityGroup) => new { Country = country, Cities = cityGroup }
                )
                .SelectMany(
                    countryCity => countryCity.Cities.DefaultIfEmpty(),
                    (countryCity, city) => new { countryCity.Country, CityId = city?.CityId }
                )
                .GroupJoin(
                    appointments,
                    countryCity => countryCity.CityId,
                    appointment => appointment.CustomerId,
                    (countryCity, appointmentGroup) => new
                    {
                        countryCity.Country.CountryName,
                        AppointmentCount = appointmentGroup.Count()
                    }
                )
                .GroupBy(result => result.CountryName)
                .Select(group => new AppointmentCountByCountry
                {
                    CountryName = group.Key,
                    AppointmentCount = group.Sum(g => g.AppointmentCount)
                })
                .ToList();

            return output;
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
