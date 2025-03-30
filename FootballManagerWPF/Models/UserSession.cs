using System;
using System.Collections.Generic;

namespace FootballManagerWPF.Models;

public partial class UserSession
{
    public int SessionId { get; set; }

    public int UserId { get; set; }

    public string? SessionData { get; set; }

    public DateTime LastUpdated { get; set; }

    public virtual User User { get; set; } = null!;
}
