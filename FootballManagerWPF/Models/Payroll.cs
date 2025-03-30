using System;
using System.Collections.Generic;

namespace FootballManagerWPF.Models;

public partial class Payroll
{
    public int PayrollId { get; set; }

    public int EmployeeId { get; set; }

    public decimal BaseSalary { get; set; }

    public decimal Allowance { get; set; }

    public decimal Bonus { get; set; }

    public decimal Penalty { get; set; }

    public decimal? TotalIncome { get; set; }

    public DateOnly PayrollDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Employee Employee { get; set; } = null!;
}
