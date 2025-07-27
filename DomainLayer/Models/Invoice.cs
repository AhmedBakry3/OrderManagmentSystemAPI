

namespace DomainLayer.Models
{
    public class Invoice : BaseEntity<int>  
    {
        public int OrderId { get; set; }

        public DateTime InvoiceDate { get; set; }

        public decimal TotalAmount { get; set; }

        // Navigation property
        public Order Order { get; set; }
    }
}
