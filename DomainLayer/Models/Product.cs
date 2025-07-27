

namespace DomainLayer.Models
{
    public class Product : BaseEntity<int>
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public int Stock { get; set; }

        // Navigation property
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
