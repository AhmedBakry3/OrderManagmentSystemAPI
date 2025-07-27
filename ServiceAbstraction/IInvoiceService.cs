


namespace ServiceAbstraction
{
    public interface IInvoiceService
    {
        Task<InvoiceDto> GetInvoiceByIdAsync(int invoiceId);
        Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync();
    }
}
