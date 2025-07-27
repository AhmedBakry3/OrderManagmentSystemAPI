



namespace Service
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {
            Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(AssemblyReference).Assembly));
            Services.AddScoped<IServiceManager, ServiceManagerWithFactoryDelegate>();
            Services.AddScoped<IMailService, MailService>();

            Services.AddScoped<IProductService, ProductService>();
            Services.AddScoped<Func<IProductService>>(Provider =>
            () => Provider.GetRequiredService<IProductService>());

            Services.AddScoped<IInvoiceService, InvoiceService>();
            Services.AddScoped<Func<IInvoiceService>>(Provider =>
            () => Provider.GetRequiredService<IInvoiceService>());

            Services.AddScoped<IAuthenticationService, AuthenticationService>();
            Services.AddScoped<Func<IAuthenticationService>>(Provider =>
            () => Provider.GetRequiredService<IAuthenticationService>());

            Services.AddScoped<ICustomerService, CustomerService>();
            Services.AddScoped<Func<ICustomerService>>(Provider =>
            () => Provider.GetRequiredService<ICustomerService>());

            Services.AddScoped<IOrderService, OrderService>();
            Services.AddScoped<Func<IOrderService>>(Provider =>
            () => Provider.GetRequiredService<IOrderService>());

            return Services;
        }
    }
}
