using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FootballManagerWPF.Models;

namespace FootballManagerWPF.Services
{
    public interface IActivityLogService
    {
        public void LogActivity(ActivityLog log);
        List<ActivityLog> GetAllActivityLogs();
    }
}
