using System;
using System.Collections.Generic;

namespace task_Nostalgia_DatabaseFirst.Models;

public partial class Listing
{
    public int ListingId { get; set; }

    public int SellerId { get; set; }

    public int CategoryId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public string Status { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<ListingImage> ListingImages { get; set; } = new List<ListingImage>();

    public virtual User Seller { get; set; } = null!;
}
