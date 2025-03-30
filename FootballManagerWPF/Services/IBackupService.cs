using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballManagerWPF.Services
{
    public interface IBackupService
    {
        void BackupEmployees(string filePath);
        void RestoreEmployees(string filePath);
    }
}
