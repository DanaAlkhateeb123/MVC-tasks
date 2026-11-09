using System;
using System.Collections.Generic;

namespace task_Nostalgia_DatabaseFirst.Models;

public partial class UserSetting
{
    public int SettingsId { get; set; }

    public int UserId { get; set; }

    public bool OrderUpdates { get; set; }

    public bool AuctionUpdates { get; set; }

    public virtual User User { get; set; } = null!;
}
