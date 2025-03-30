using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FootballManagerWPF.Models;
using FootballManagerWPF.Services;
using Newtonsoft.Json;
using System.IO;

namespace FootballManagerWPF.Services
{
    public class SessionStorageService
    {
        private readonly string _filePath = "session.json";

        public void SaveSession(SessionData session)
        {
            try
            {
                string json = JsonConvert.SerializeObject(session, Formatting.Indented);
                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                // Ghi log lỗi hoặc xử lý theo yêu cầu
                throw new Exception("Lỗi khi lưu phiên làm việc: " + ex.Message);
            }
        }

        public SessionData LoadSession()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    string json = File.ReadAllText(_filePath);
                    SessionData session = JsonConvert.DeserializeObject<SessionData>(json);
                    return session;
                }
                else
                {
                    return new SessionData();
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi, có thể trả về phiên làm việc rỗng
                throw new Exception("Lỗi khi tải phiên làm việc: " + ex.Message);
            }
        }
    }
}
