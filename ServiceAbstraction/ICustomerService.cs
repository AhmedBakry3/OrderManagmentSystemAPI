


namespace ServiceAbstraction
{
    public interface ICustomerService
    {
        Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto createCustomerDto);
        Task<CustomerDto> GetCustomerByIdAsync(int customerId);
        Task<List<CustomerDto>> GetAllCustomersAsync();
    }
}
