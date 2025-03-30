using System;
using System.Collections.Generic;

namespace FootballManagerWPF.Models;

public partial class LeaveRequest
{
    public int LeaveId { get; set; }

    public int EmployeeId { get; set; }

    public string LeaveType { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public int? TotalDays { get; set; }

    public string Status { get; set; } = null!;

    public DateTime RequestedAt { get; set; }

    public int? ApprovedBy { get; set; }

    public DateTime? ApprovedAt { get; set; }

    public virtual User? ApprovedByNavigation { get; set; }

    public virtual Employee Employee { get; set; } = null!;
}
