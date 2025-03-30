using System;
using System.Collections.Generic;

namespace FootballManagerWPF.Models;

public partial class BackupHistory
{
    public int BackupId { get; set; }

    public DateTime BackupDate { get; set; }

    public string FileName { get; set; } = null!;

    public int? CreatedBy { get; set; }

    public virtual User? CreatedByNavigation { get; set; }
}
