

namespace DomainLayer.Contracts
{
    public interface IJwtGenerator
    {
        string GenerateToken(IdentityUser user);

    }
}
