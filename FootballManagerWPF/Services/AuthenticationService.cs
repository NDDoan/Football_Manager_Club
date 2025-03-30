using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FootballManagerWPF.Helpers;
using FootballManagerWPF.Models;
using Microsoft.Data.SqlClient;

namespace FootballManagerWPF.Services
{
    public class AuthenticationService
    {
        private readonly string _connectionString;

        public AuthenticationService()
        {
            _connectionString = ConfigurationHelper.GetConnectionString("DefaultConnection");
        }

        public bool ValidateUser(string username, string password, out string role, out int userId)
        {
            role = null;
            userId = 0;
            if (string.IsNullOrWhiteSpace(username))
            {
                return false;
            }
            string query = "SELECT user_id, password, role FROM dbo.Users WHERE username = @username";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string storedPassword = reader["password"].ToString();
                            if (password == storedPassword)  // So sánh password, nên hash trong thực tế
                            {
                                userId = Convert.ToInt32(reader["user_id"]);
                                role = reader["role"].ToString();

                                // Log đăng nhập
                                ActivityLogService logService = new ActivityLogService();
                                logService.LogActivity(new ActivityLog
                                {
                                    UserId = userId,
                                    Activity = "LOGIN",
                                    TableName = "Users",
                                    RecordId = userId,
                                    Details = $"User '{username}' logged in successfully."
                                });

                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}
