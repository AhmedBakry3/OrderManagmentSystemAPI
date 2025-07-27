


using Shared.Enum;

namespace Shared.DataTransferObject.OrderModuleDTos
{
    public class CreateOrderDto
    {
        public int CustomerId { get; set; }
        public List<CreateOrderItemDto> OrderItems { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
