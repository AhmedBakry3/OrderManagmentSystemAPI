


namespace ServiceLayer.Tests
{
    public class CustomerServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CustomerService _customerService;

        public CustomerServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _customerService = new CustomerService(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task CreateCustomerAsync_ShouldCreateCustomerSuccessfully()
        {
            var createCustomerDto = new CreateCustomerDto
            {
                Name = "Ahmed Bakry",
                Email = "ahmed.bakry@example.com"
            };

            var customer = new Customer
            {
                Id = 1,
                Name = "Ahmed Bakry",
                Email = "ahmed.bakry@example.com"
            };

            _mockMapper.Setup(m => m.Map<Customer>(createCustomerDto)).Returns(customer);
            _mockUnitOfWork.Setup(u => u.GetRepository<Customer, int>().AddAsync(customer)).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);
            _mockMapper.Setup(m => m.Map<CustomerDto>(customer)).Returns(new CustomerDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email
            });

            var result = await _customerService.CreateCustomerAsync(createCustomerDto);

            Assert.NotNull(result);
            Assert.Equal(createCustomerDto.Name, result.Name);
            _mockUnitOfWork.Verify(u => u.GetRepository<Customer, int>().AddAsync(customer), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetCustomerByIdAsync_ShouldReturnCustomer_WhenCustomerExists()
        {
            var customerId = 1;
            var customer = new Customer { Id = customerId, Name = "Ahmed Bakry", Email = "ahmed.bakry@example.com" };

            _mockUnitOfWork.Setup(u => u.GetRepository<Customer, int>().GetByIdAsync(customerId)).ReturnsAsync(customer);
            _mockMapper.Setup(m => m.Map<CustomerDto>(customer)).Returns(new CustomerDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email
            });

            var result = await _customerService.GetCustomerByIdAsync(customerId);

            Assert.NotNull(result);
            Assert.Equal(customerId, result.Id);
            _mockUnitOfWork.Verify(u => u.GetRepository<Customer, int>().GetByIdAsync(customerId), Times.Once);
        }

        [Fact]
        public async Task GetCustomerByIdAsync_ShouldThrowCustomerNotFoundException_WhenCustomerDoesNotExist()
        {
            var customerId = 1;
            _mockUnitOfWork.Setup(u => u.GetRepository<Customer, int>().GetByIdAsync(customerId)).ReturnsAsync((Customer)null);

            await Assert.ThrowsAsync<CustomerNotFoundException>(() => _customerService.GetCustomerByIdAsync(customerId));
        }

        [Fact]
        public async Task GetAllCustomersAsync_ShouldReturnListOfCustomers()
        {
            var customers = new List<Customer>
            {
                new Customer { Id = 1, Name = "Ahmed Bakry", Email = "ahmed.bakry@example.com" },
                new Customer { Id = 2, Name = "Mohamed Bakry", Email = "mohamed.bakry@example.com" }
            };

            _mockUnitOfWork.Setup(u => u.GetRepository<Customer, int>().GetAllAsync()).ReturnsAsync(customers);
            _mockMapper.Setup(m => m.Map<List<CustomerDto>>(customers)).Returns(new List<CustomerDto>
            {
                new CustomerDto { Id = 1, Name = "Ahmed Bakry", Email = "ahmed.bakry@example.com" },
                new CustomerDto { Id = 2, Name = "Mohamed Bakry", Email = "mohamed.bakry@example.com" }
            });

            var result = await _customerService.GetAllCustomersAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            _mockUnitOfWork.Verify(u => u.GetRepository<Customer, int>().GetAllAsync(), Times.Once);
        }
    }
}
