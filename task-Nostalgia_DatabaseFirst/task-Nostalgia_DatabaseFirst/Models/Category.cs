using System;
using System.Collections.Generic;

namespace task_Nostalgia_DatabaseFirst.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<Listing> Listings { get; set; } = new List<Listing>();
}
