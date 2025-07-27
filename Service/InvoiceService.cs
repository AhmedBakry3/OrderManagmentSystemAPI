using Shared.DataTransferObject.InvoiceModuleDTos;
using static Service.Specifications.InvoiceModuleSpecifications.InvoiceSpecifications;

namespace ServiceLayer
{
    public class InvoiceService(IUnitOfWork _unitOfWork, IMapper _mapper) : IInvoiceService
    {
        public async Task<InvoiceDto> GetInvoiceByIdAsync(int invoiceId)
        {
            var spec = new InvoiceWithOrderSpecification(invoiceId);
            var invoice = await _unitOfWork
                .GetRepository<Invoice, int>()
                .GetByIdAsync(spec);

            if (invoice == null)
                throw new InvoiceNotFoundException(invoiceId);

            return _mapper.Map<InvoiceDto>(invoice);
        }

        public async Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync()
        {
            var spec = new InvoiceWithOrderSpecification();
            var invoices = await _unitOfWork
                .GetRepository<Invoice, int>()
                .GetAllAsync(spec);

            return _mapper.Map<IEnumerable<InvoiceDto>>(invoices);
        }
    }
}
