


namespace ServiceAbstraction
{
    public interface IServiceManager
    {
        public ICustomerService CustomerService { get; }
        public IProductService productService { get; }

        public IInvoiceService InvoiceService { get; }

        public IAuthenticationService AuthenticationService { get; }

        public IOrderService OrderService { get; }


    }
}
