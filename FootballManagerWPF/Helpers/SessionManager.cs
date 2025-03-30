using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballManagerWPF.Helpers
{
    public static class SessionManager
    {
        // Lưu trữ UserId của người dùng đã đăng nhập
        public static int LoggedInUserId { get; set; }
    }

}
