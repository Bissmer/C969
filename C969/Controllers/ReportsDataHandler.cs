using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C969.Models;
using MySql.Data.MySqlClient;
using C969.Controllers;

namespace C969.Controllers
{
    public class ReportsDataHandler
    {
        private readonly MySqlConnection _connection;

        private readonly string _connString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;

        private string _currentUser;
        private readonly CustomerDataHandler _customerDataHandler;

        public ReportsDataHandler(string currentUser)
        {
            _currentUser = currentUser;
            _connection = new MySqlConnection(_connString);
            _customerDataHandler = new CustomerDataHandler(currentUser);
        }

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


    }
}
