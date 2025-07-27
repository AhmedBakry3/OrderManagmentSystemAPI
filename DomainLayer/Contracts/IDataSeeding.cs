

namespace DomainLayer.Contracts
{
    public interface IDataSeeding
    {
        Task IdentityDataSeedAsync();

        Task SeedInvoicesAsync();
    }
}
