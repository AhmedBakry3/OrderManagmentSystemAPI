



namespace Presentation.Controllers
{
    public class CustomersController(IServiceManager _serviceManager) : ApiBaseController
    {
        // POST: api/customers
        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync([FromBody] CreateCustomerDto createCustomerDto)
        {
            var customerDto = await _serviceManager.CustomerService.CreateCustomerAsync(createCustomerDto);
            return Ok(customerDto);
        }

        //GET: api/customers/{customerId}/orders
        [HttpGet("{customerId}/orders")]
        public async Task<IActionResult> GetOrdersForCustomer(int customerId)
        {
        var ordersDto = await _serviceManager.OrderService.GetOrdersByCustomerIdAsync(customerId);
        return Ok(ordersDto);
        }
    }
}
