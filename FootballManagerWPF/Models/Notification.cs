using System;
using System.Collections.Generic;

namespace FootballManagerWPF.Models;

public partial class Notification
{
    public int NotificationId { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public int? DepartmentId { get; set; }

    public int? SenderUserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Department? Department { get; set; }

    public virtual User? SenderUser { get; set; }
}
