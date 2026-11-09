using System;
using System.Collections.Generic;

namespace task_Nostalgia_DatabaseFirst.Models;

public partial class ListingImage
{
    public int ImageId { get; set; }

    public int ListingId { get; set; }

    public string ImagePath { get; set; } = null!;

    public virtual Listing Listing { get; set; } = null!;
}
