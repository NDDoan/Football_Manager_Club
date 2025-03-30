using System;
using System.Collections.Generic;

namespace FootballManagerWPF.Models;

public partial class Attendance
{
    public int AttendanceId { get; set; }

    public int EmployeeId { get; set; }

    public DateOnly AttendanceDate { get; set; }

    public DateTime? CheckIn { get; set; }

    public DateTime? CheckOut { get; set; }

    public decimal OvertimeHours { get; set; }

    public string LeaveType { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual Employee Employee { get; set; } = null!;
}
