using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications.InvoiceModuleSpecifications
{
    public class InvoiceSpecifications
    {
        public class InvoiceWithOrderSpecification : BaseSpecifications<Invoice, int>
        {
            public InvoiceWithOrderSpecification(int? invoiceId = null)
                : base(invoiceId.HasValue ? x => x.Id == invoiceId.Value : null)
            {
                AddInclude(invoice => invoice.Order);
            }
        }
    }
}
