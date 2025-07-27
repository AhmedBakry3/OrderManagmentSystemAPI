


namespace Persistence.Data.Configurations
{
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            // Primary Key
            builder.HasKey(i => i.Id);

            // Required fields
            builder.Property(i => i.InvoiceDate)
                .IsRequired();

            builder.Property(i => i.TotalAmount)
                .IsRequired(); 

            // One-to-one relationship with Order
            builder.HasOne(i => i.Order)
                .WithOne(o => o.Invoice)
                .HasForeignKey<Invoice>(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade);  
        }
    }
}
