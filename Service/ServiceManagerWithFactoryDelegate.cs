

namespace Service
{
    public class ServiceManagerWithFactoryDelegate(Func<ICustomerService> CustomerFactory
        ,Func<IProductService> ProductFactory
        ,Func<IInvoiceService> InvoiceFactory
        ,Func<IAuthenticationService> AuthenticationFactory
        ,Func<IOrderService> OrderFactory) : IServiceManager
    {
        public ICustomerService CustomerService => CustomerFactory.Invoke();

        public IProductService productService => ProductFactory.Invoke();

        public IInvoiceService InvoiceService => InvoiceFactory.Invoke();

        public IAuthenticationService AuthenticationService => AuthenticationFactory.Invoke();

        public IOrderService OrderService => OrderFactory.Invoke();

    }
}
