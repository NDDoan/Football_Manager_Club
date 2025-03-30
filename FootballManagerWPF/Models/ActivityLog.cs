using System;
using System.Collections.Generic;

namespace FootballManagerWPF.Models;

public partial class ActivityLog
{
    public int LogId { get; set; }

    public int UserId { get; set; }

    public string Activity { get; set; } = null!;

    public string? TableName { get; set; }

    public int? RecordId { get; set; }

    public string? Details { get; set; }

    public DateTime ActivityTimestamp { get; set; }

    public virtual User User { get; set; } = null!;
}
