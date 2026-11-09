namespace task66.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }

        public Category? category { get; set; }
        public Supplier? supplier { get; set; }

        public int CategoryId { get; set; }
        public int SupplierId{ get; set; }


    }
}
