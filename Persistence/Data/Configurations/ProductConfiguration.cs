



namespace Persistence.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // Primary Key
            builder.HasKey(p => p.Id);

            // Required fields
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);  

            builder.Property(p => p.Price)
                .IsRequired();  

            builder.Property(p => p.Stock)
                .IsRequired();  

            // Ensure Name is unique
            builder.HasIndex(p => p.Name).IsUnique();  
        }
    }
}
