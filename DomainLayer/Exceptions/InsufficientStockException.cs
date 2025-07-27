using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Exceptions
{
    public class InsufficientStockException : Exception
    {
        public int ProductId { get; }
        public int RequestedQuantity { get; }
        public int AvailableStock { get; }

        public InsufficientStockException(int productId, int requestedQuantity, int availableStock)
            : base($"Product with ID {productId} has insufficient stock. Requested quantity: {requestedQuantity}, Available stock: {availableStock}")
        {
            ProductId = productId;
            RequestedQuantity = requestedQuantity;
            AvailableStock = availableStock;
        }
    }
}