using System.ComponentModel.DataAnnotations;

namespace task_Nostalgia_CodeFirst.Models
{
    public class Listing
    {
        [Key]
        public int ListingID { get; set; }

        public int SellerID { get; set; }

        public int CategoryID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string Status { get; set; } = "Pending";

        public User Seller { get; set; }

        public Category Category { get; set; }

        public ICollection<ListingImage> ListingImages { get; set; }
    }
}