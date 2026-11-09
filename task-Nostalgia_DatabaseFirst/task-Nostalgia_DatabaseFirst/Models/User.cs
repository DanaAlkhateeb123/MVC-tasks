using System;
using System.Collections.Generic;

namespace task_Nostalgia_DatabaseFirst.Models;

public partial class User
{
    public int UserId { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public virtual ICollection<Listing> Listings { get; set; } = new List<Listing>();

    public virtual UserSetting? UserSetting { get; set; }
}
