using System.ComponentModel.DataAnnotations;

namespace task_Nostalgia_CodeFirst.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        public string CategoryName { get; set; }

        public ICollection<Listing> Listings { get; set; }
    }
}