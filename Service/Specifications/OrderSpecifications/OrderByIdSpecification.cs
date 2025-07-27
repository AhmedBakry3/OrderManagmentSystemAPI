


namespace Service.Specifications.OrderSpecifications
{
    public class OrderByIdSpecification : BaseSpecifications<Order, int>
    {
        public OrderByIdSpecification(int orderId) : base(o => o.Id == orderId)
        {
            AddInclude(o => o.Customer);
            AddInclude(o => o.OrderItems);

        }
    }
}
