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
    public class NotificationService : INotificationService
    {
        private readonly string _connectionString;

        public NotificationService()
        {
            _connectionString = ConfigurationHelper.GetConnectionString("DefaultConnection");
        }

        public List<Notification> GetNotifications()
        {
            var notifications = new List<Notification>();
            string query = @"SELECT notification_id, title, content, department_id, sender_user_id, created_at
                             FROM dbo.Notification";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var cmd = new SqlCommand(query, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        notifications.Add(new Notification
                        {
                            NotificationId = Convert.ToInt32(reader["notification_id"]),
                            Title = reader["title"].ToString(),
                            Content = reader["content"].ToString(),
                            DepartmentId = reader["department_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["department_id"]),
                            SenderUserId = reader["sender_user_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["sender_user_id"]),
                            CreatedAt = Convert.ToDateTime(reader["created_at"])
                        });
                    }
                }
            }
            return notifications;
        }

        public void SendNotification(Notification notification)
        {
            string query = @"
                INSERT INTO dbo.Notification (title, content, department_id, sender_user_id, created_at)
                VALUES (@Title, @Content, @DepartmentId, @SenderUserId, GETDATE())";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Title", notification.Title);
                    cmd.Parameters.AddWithValue("@Content", notification.Content);
                    cmd.Parameters.AddWithValue("@DepartmentId", (object)notification.DepartmentId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@SenderUserId", notification.SenderUserId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Notification GetLatestAdminNotification()
        {
            // Giả sử admin đăng thông báo có sender_user_id khác null.
            // Lọc thông báo trong 72 giờ gần đây và lấy thông báo mới nhất
            string query = @"
                SELECT TOP 1 notification_id, title, content, created_at 
                FROM dbo.Notification 
                WHERE created_at >= DATEADD(HOUR, -72, GETDATE())
                AND sender_user_id IS NOT NULL
                ORDER BY created_at DESC";

            using (var connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                using (var cmd = new SqlCommand(query, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Notification
                            {
                                NotificationId = Convert.ToInt32(reader["notification_id"]),
                                Title = reader["title"].ToString(),
                                Content = reader["content"].ToString(),
                                CreatedAt = Convert.ToDateTime(reader["created_at"])
                            };
                        }
                    }
                }
            }
            return null;
        }
        public List<Notification> GetNotificationsForEmployee(int? departmentId = null)
        {
            var notifications = new List<Notification>();
            string query = @"
        SELECT notification_id, title, content, department_id, sender_user_id, created_at
        FROM dbo.Notification
        WHERE (@DepartmentId IS NULL OR department_id = @DepartmentId)
        ORDER BY created_at DESC";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@DepartmentId", (object)departmentId ?? DBNull.Value);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            notifications.Add(new Notification
                            {
                                NotificationId = Convert.ToInt32(reader["notification_id"]),
                                Title = reader["title"].ToString(),
                                Content = reader["content"].ToString(),
                                DepartmentId = reader["department_id"] == DBNull.Value ? null : (int?)Convert.ToInt32(reader["department_id"]),
                                // Kiểm tra sender_user_id trước khi chuyển đổi
                                SenderUserId = reader["sender_user_id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["sender_user_id"]),
                                CreatedAt = Convert.ToDateTime(reader["created_at"])
                            });
                        }
                    }
                }
            }
            return notifications;
        }
    }
}