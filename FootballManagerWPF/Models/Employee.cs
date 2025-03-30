using System;
using System.Collections.Generic;

namespace FootballManagerWPF.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public int? UserId { get; set; }

    public string FullName { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public string Gender { get; set; } = null!;

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public int? DepartmentId { get; set; }

    public string? Position { get; set; }

    public decimal? BaseSalary { get; set; }

    public DateOnly? StartDate { get; set; }

    public string? Avatar { get; set; }

    public int AnnualLeaveBalance { get; set; }

    public int SickLeaveBalance { get; set; }

    public int UnpaidLeaveBalance { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual Department? Department { get; set; }

    public virtual ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();

    public virtual ICollection<Payroll> Payrolls { get; set; } = new List<Payroll>();

    public virtual User? User { get; set; }
}
