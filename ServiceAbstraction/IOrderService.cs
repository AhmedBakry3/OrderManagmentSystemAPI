

namespace ServiceAbstraction
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto);                     
        Task<OrderDto> GetOrderByIdAsync(int orderId);                                     
        Task<List<OrderDto>> GetOrdersByCustomerIdAsync(int customerId);                 
        Task<List<OrderDto>> GetAllOrdersAsync();
        Task UpdateOrderStatusAsync(int orderId, string newStatus);
    }
}
