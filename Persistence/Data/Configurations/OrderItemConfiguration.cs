



namespace Persistence.Data.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            // Primary Key
            builder.HasKey(oi => oi.Id);

            // Required fields
            builder.Property(oi => oi.Quantity)
                .IsRequired(); 

            builder.Property(oi => oi.UnitPrice)
                .IsRequired();

            builder.Property(oi => oi.Discount)
                .IsRequired();

            // One-to-many relationship with Order
            builder.HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade); 

            // One-to-many relationship with Product
            builder.HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Composite Unique Index
            builder.HasIndex(oi => new { oi.OrderId, oi.ProductId })
                .IsUnique(); 
        }
    }
}
