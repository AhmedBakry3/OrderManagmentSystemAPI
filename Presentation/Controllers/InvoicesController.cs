using Microsoft.AspNetCore.Authorization;
using ServiceAbstraction;
using Shared.DataTransferObject.InvoiceModuleDTos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Authorize(Roles = "Admin")] // Admin-only
    public class InvoicesController(IServiceManager _serviceManager) : ApiBaseController
    {
        // GET: BaseUrl/api/Invoices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetAllInvoicesAsync()
        {
            var invoices = await _serviceManager.InvoiceService.GetAllInvoicesAsync();
            return Ok(invoices);
        }

        // GET: BaseUrl/api/Invoices/invoiceId
        [HttpGet("{invoiceId}")]
        public async Task<ActionResult<InvoiceDto>> GetInvoiceByIdAsync(int invoiceId)
        {
            var invoice = await _serviceManager.InvoiceService.GetInvoiceByIdAsync(invoiceId);
            return Ok(invoice);
        }
    }
}
