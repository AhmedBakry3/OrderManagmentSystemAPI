

using Microsoft.AspNetCore.Authorization;
using Shared.DataTransferObject.OrderModuleDTos;

namespace Presentation.Controllers
{
    public class OrdersController(IServiceManager _serviceManager) : ApiBaseController
    {
        //POST : BaseUrl/api/Orders
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
        {
            var createdOrder = await _serviceManager.OrderService.CreateOrderAsync(dto);
            return CreatedAtAction(nameof(GetOrderById), new { orderId = createdOrder.Id }, createdOrder);
        }

        //POST : BaseUrl/api/Orders/{orderId}
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var order = await _serviceManager.OrderService.GetOrderByIdAsync(orderId);
            return Ok(order);
        }

        //GET : BaseUrl/api/Orders
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _serviceManager.OrderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        //PUT : BaseUrl/api/Orders/{orderId}/status
        [Authorize(Roles = "Admin")]
        [HttpPut("{orderId}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromQuery] string newStatus)
        {
            await _serviceManager.OrderService.UpdateOrderStatusAsync(orderId, newStatus);
            return Ok("Order status updated successfully.");
        }

    }
}
