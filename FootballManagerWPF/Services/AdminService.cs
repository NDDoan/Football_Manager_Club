using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FootballManagerWPF.Helpers;
using Microsoft.Data.SqlClient;

namespace FootballManagerWPF.Services
{
    public class AdminService
    {
        private readonly string _connectionString;

        public AdminService()
        {
            _connectionString = ConfigurationHelper.GetConnectionString("DefaultConnection");
        }

        /// <summary>
        /// Lấy thông tin admin (FullName, Email, Avatar) từ bảng Employee và Users.
        /// </summary>
        public (string FullName, string Email, string Avatar) GetAdminInfo()
        {
            string query = @"
                SELECT TOP 1 
                    e.full_name,
                    e.avatar,
                    u.username AS Email
                FROM dbo.Employee e
                INNER JOIN dbo.Users u ON e.user_id = u.user_id
                WHERE u.role = 'Admin'";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string fullName = reader["full_name"].ToString();
                            string avatar = reader["avatar"].ToString();
                            string email = reader["Email"].ToString();
                            return (fullName, email, avatar);
                        }
                    }
                }
            }
            return (null, null, null);
        }
    }
}
