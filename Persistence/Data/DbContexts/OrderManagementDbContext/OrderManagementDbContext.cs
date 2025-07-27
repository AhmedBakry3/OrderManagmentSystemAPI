



namespace Persistence.Data.DbContexts.OrderManagementDbContext
{
    public class OrderManagementDbContext(DbContextOptions<OrderManagementDbContext> Options) : DbContext(Options)
    {
        // DbSet properties for each entity 
        public DbSet<Customer> Customers { get; set; }

        public DbSet<DomainLayer.Models.Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Invoice> Invoices { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AssemblyReference).Assembly);
        }
    }
}
