using System.ComponentModel.DataAnnotations;

namespace task_Nostalgia_CodeFirst.Models
{
    public class ListingImage
    {
        [Key]
        public int ImageID { get; set; }

        public int ListingID { get; set; }

        public string ImagePath { get; set; }

        public Listing Listing { get; set; }
    }
}