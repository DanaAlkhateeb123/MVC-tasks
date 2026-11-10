using task8.Models;

namespace Task8.Models
{
    public class Product
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public ICollection<RequestItem> requestItems { get; set; }

    }
}