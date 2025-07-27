

namespace DomainLayer.Exceptions
{
    public sealed class CustomerNotFoundException(int id) : NotFoundException($"Customer with Id = {id} Is Not Found")
    {
    }
}
