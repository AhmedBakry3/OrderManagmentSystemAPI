



namespace Persistence.Data.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Primary Key
            builder.HasKey(u => u.Id);

            // Required fields
            builder.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);

            // Role is required
            builder.Property(u => u.Role)
                .IsRequired()
                .HasMaxLength(20);

            // Ensure Username is unique
            builder.HasIndex(u => u.Username)
                .IsUnique();
        }
    }
}
