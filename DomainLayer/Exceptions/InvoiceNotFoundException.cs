

namespace DomainLayer.Exceptions
{
    public sealed class InvoiceNotFoundException(int id) : NotFoundException($"Invoice with Id = {id} Is Not Found")
    {
    }
}
