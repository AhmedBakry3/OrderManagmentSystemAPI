

namespace DomainLayer.Exceptions
{
    public sealed class ProductNotFoundException(int id) : NotFoundException($"Product with Id = {id} Is Not Found")
    {
    }
}
