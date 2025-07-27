




namespace Persistence
{
    public static class InfrastructureServicesRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection Services, IConfiguration Configuration)
        {

            Services.AddScoped<IDataSeeding, DataSeeding>();
            Services.AddScoped<IUnitOfWork, UnitOfWork>();

            //In-memory database Configuration (Redis)
            Services.AddSingleton<IConnectionMultiplexer>((_) =>
            {
                return ConnectionMultiplexer.Connect(Configuration.GetConnectionString("RedisConnectionString"));
            });

            //OrderManagementDbContext Configuration
            Services.AddDbContext<OrderManagementDbContext>(Options =>
            {
                Options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            //StoredIdentityDbContext Configuration
            Services.AddDbContext<StoredIdentityDbContext>(Options =>
            {
                Options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection"));
            });

            Services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddEntityFrameworkStores<StoredIdentityDbContext>()
                    .AddDefaultTokenProviders();


            Services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));

            return Services;
        }
    }
}
