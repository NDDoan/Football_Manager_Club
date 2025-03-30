using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballManagerWPF.Models
{
    public class PayrollRecord
    {
        public int PayrollId { get; set; }
        public int EmployeeId { get; set; }
        public decimal BaseSalary { get; set; }
        public decimal Allowance { get; set; }
        public decimal Bonus { get; set; }
        public decimal Penalty { get; set; }
        public decimal TotalIncome { get; set; }
        public DateTime PayrollDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
