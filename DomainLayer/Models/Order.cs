

using Shared.Enum;

namespace DomainLayer.Models
{
    public class Order : BaseEntity<int>    
    {
        public int CustomerId { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public OrderStatus Status { get; set; }

        // Navigation properties
        public Customer Customer { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public Invoice Invoice { get; set; }
    }
}
