



namespace Persistence.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<DomainLayer.Models.Order>
    {
        public void Configure(EntityTypeBuilder<DomainLayer.Models.Order> builder)
        {
            // Primary Key
            builder.HasKey(o => o.Id);

            // Required fields
            builder.Property(o => o.OrderDate)
                .IsRequired();  

            builder.Property(o => o.TotalAmount)
                .IsRequired();  

            builder.Property(o => o.PaymentMethod)
                .IsRequired(); 

            builder.Property(o => o.Status)
                .IsRequired();

            // Store the PaymentMethod enum as a string in the database
            builder.Property(o => o.PaymentMethod)
                  .HasConversion<string>();

            // Store the Status enum as a string in the database
            builder.Property(o => o.Status)
                  .HasConversion<string>();

            // One-to-many relationship with OrderItems
            builder.HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-many relationship with Customer
            builder.HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId);
        }
    }
}
