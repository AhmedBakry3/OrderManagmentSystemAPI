


namespace Service.Specifications.OrderSpecifications
{
    public class AllOrdersSpecifications : BaseSpecifications<Order, int>
    {
        public AllOrdersSpecifications() : base(null)
        {
            AddInclude(o => o.Customer);
        }
    }
}
