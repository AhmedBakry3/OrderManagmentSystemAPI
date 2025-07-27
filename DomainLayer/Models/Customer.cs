

namespace DomainLayer.Models
{
    public class Customer : BaseEntity<int>
    {
        public string Name { get; set; }

        public string Email { get; set; }

        // Navigation property
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
