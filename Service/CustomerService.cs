

namespace ServiceLayer
{
    public class CustomerService(IUnitOfWork _unitOfWork , IMapper _mapper) : ICustomerService
    {
        public async Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto createCustomerDto)
        {
            var customer = _mapper.Map<Customer>(createCustomerDto);

            await _unitOfWork.GetRepository<Customer, int>().AddAsync(customer);
            await _unitOfWork.SaveChangesAsync();

            var customerDto = _mapper.Map<CustomerDto>(customer);
            return customerDto;
        }

        public async Task<CustomerDto> GetCustomerByIdAsync(int customerId)
        {
            var customer = await _unitOfWork.GetRepository<Customer, int>().GetByIdAsync(customerId);
            if (customer == null)
                throw new CustomerNotFoundException(customerId);

            var customerDto = _mapper.Map<CustomerDto>(customer);
            return customerDto;
        }

        public async Task<List<CustomerDto>> GetAllCustomersAsync()
        {
            var customers = await _unitOfWork.GetRepository<Customer, int>().GetAllAsync();
            var customerDtos = _mapper.Map<List<CustomerDto>>(customers);
            return customerDtos;
        }
    }
}
