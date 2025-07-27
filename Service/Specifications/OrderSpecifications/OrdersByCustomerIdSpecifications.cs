

namespace Service.Specifications.OrderSpecifications
{
    public class OrdersByCustomerIdSpecifications : BaseSpecifications<Order, int>
    {
        public OrdersByCustomerIdSpecifications(int customerId) : base(o => o.CustomerId == customerId)
        {
            AddInclude(o => o.Customer);
        }
    }
}
