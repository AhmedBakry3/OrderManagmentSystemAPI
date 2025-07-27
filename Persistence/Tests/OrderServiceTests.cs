

using Moq;

namespace ServiceLayer.Tests
{
    public class OrderServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IMailService> _mockMailService;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockMailService = new Mock<IMailService>();

            _orderService = new OrderService(_mockUnitOfWork.Object, _mockMapper.Object, _mockMailService.Object);
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldReturnOrderDto_WhenOrderCreatedSuccessfully()
        {
            var createOrderDto = new CreateOrderDto
            {
                CustomerId = 1,
                OrderItems = new List<CreateOrderItemDto>
        {
            new CreateOrderItemDto { ProductId = 1, Quantity = 2, UnitPrice = 100, Discount = 0 }
        },
                PaymentMethod = PaymentMethod.CreditCard
            };

            var customer = new Customer { Id = 1, Email = "ahmed.elbakry.ab@gmail.com", Name = "AhmedBakry" };
            var product = new Product { Id = 1, Stock = 10 };
            var invoice = new Invoice { OrderId = 1, InvoiceDate = DateTime.UtcNow, TotalAmount = 200 };

            _mockUnitOfWork.Setup(u => u.GetRepository<Customer, int>().GetByIdAsync(createOrderDto.CustomerId))
                .ReturnsAsync(customer); 

            var mockProductRepo = new Mock<IGenericRepository<Product, int>>();
            _mockUnitOfWork.Setup(u => u.GetRepository<Product, int>()).Returns(mockProductRepo.Object);
            mockProductRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(product); 
            mockProductRepo.Setup(r => r.Update(It.IsAny<Product>())).Verifiable(); 

            var mockOrderRepo = new Mock<IGenericRepository<DomainLayer.Models.Order, int>>();
            _mockUnitOfWork.Setup(u => u.GetRepository<DomainLayer.Models.Order, int>()).Returns(mockOrderRepo.Object);
            mockOrderRepo.Setup(r => r.AddAsync(It.IsAny<DomainLayer.Models.Order>())).Returns(Task.CompletedTask); 

            var mockInvoiceRepo = new Mock<IGenericRepository<Invoice, int>>();
            _mockUnitOfWork.Setup(u => u.GetRepository<Invoice, int>()).Returns(mockInvoiceRepo.Object);
            mockInvoiceRepo.Setup(r => r.AddAsync(It.IsAny<Invoice>())).Returns(Task.CompletedTask); 

            var orderDto = new OrderDto { Id = 1, TotalAmount = 200 };
            _mockMapper.Setup(m => m.Map<OrderDto>(It.IsAny<DomainLayer.Models.Order>())).Returns(orderDto);  
            _mockMailService.Setup(m => m.SendAsync(It.IsAny<Email>())).Returns(Task.CompletedTask);  

            var result = await _orderService.CreateOrderAsync(createOrderDto);

            Assert.NotNull(result);
            Assert.Equal(orderDto.Id, result.Id);
            Assert.Equal(orderDto.TotalAmount, result.TotalAmount);

            _mockUnitOfWork.Verify(u => u.GetRepository<DomainLayer.Models.Order, int>().AddAsync(It.IsAny<DomainLayer.Models.Order>()), Times.Once); 
            _mockUnitOfWork.Verify(u => u.GetRepository<Product, int>().Update(It.IsAny<Product>()), Times.Once); 
            _mockUnitOfWork.Verify(u => u.GetRepository<Invoice, int>().AddAsync(It.IsAny<Invoice>()), Times.Once); 
            _mockMailService.Verify(m => m.SendAsync(It.IsAny<Email>()), Times.Once);  
        }




        [Fact]
        public async Task GetOrderByIdAsync_ShouldReturnOrderDto_WhenOrderExists()
        {
            var order = new DomainLayer.Models.Order { Id = 1, CustomerId = 1, TotalAmount = 200 };
            var orderDto = new OrderDto { Id = 1, TotalAmount = 200 };

            _mockUnitOfWork.Setup(u => u.GetRepository<DomainLayer.Models.Order, int>().GetByIdAsync(It.IsAny<OrderByIdSpecification>())).ReturnsAsync(order);
            _mockMapper.Setup(m => m.Map<OrderDto>(It.IsAny<DomainLayer.Models.Order>())).Returns(orderDto);

            var result = await _orderService.GetOrderByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(orderDto.Id, result.Id);
            Assert.Equal(orderDto.TotalAmount, result.TotalAmount);
        }

        [Fact]
        public async Task GetOrdersByCustomerIdAsync_ShouldReturnListOfOrders_WhenOrdersExist()
        {
            var orders = new List<DomainLayer.Models.Order>
            {
                new DomainLayer.Models.Order { Id = 1, CustomerId = 1, TotalAmount = 200 },
                new DomainLayer.Models.Order { Id = 2, CustomerId = 1, TotalAmount = 150 }
            };
            var orderDtos = new List<OrderDto>
            {
                new OrderDto { Id = 1, TotalAmount = 200 },
                new OrderDto { Id = 2, TotalAmount = 150 }
            };

            _mockUnitOfWork.Setup(u => u.GetRepository<DomainLayer.Models.Order, int>().GetAllAsync(It.IsAny<OrdersByCustomerIdSpecifications>())).ReturnsAsync(orders);
            _mockMapper.Setup(m => m.Map<List<OrderDto>>(It.IsAny<List<DomainLayer.Models.Order>>())).Returns(orderDtos);

            var result = await _orderService.GetOrdersByCustomerIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(orderDtos[0].Id, result[0].Id);
            Assert.Equal(orderDtos[1].TotalAmount, result[1].TotalAmount);
        }

        [Fact]
        public async Task GetAllOrdersAsync_ShouldReturnListOfOrders()
        {
            var orders = new List<DomainLayer.Models.Order>
            {
                new DomainLayer.Models.Order { Id = 1, TotalAmount = 200 },
                new DomainLayer.Models.Order { Id = 2, TotalAmount = 150 }
            };
            var orderDtos = new List<OrderDto>
            {
                new OrderDto { Id = 1, TotalAmount = 200 },
                new OrderDto { Id = 2, TotalAmount = 150 }
            };

            _mockUnitOfWork.Setup(u => u.GetRepository<DomainLayer.Models.Order, int>().GetAllAsync(It.IsAny<AllOrdersSpecifications>())).ReturnsAsync(orders);
            _mockMapper.Setup(m => m.Map<List<OrderDto>>(It.IsAny<List<DomainLayer.Models.Order>>())).Returns(orderDtos);

            var result = await _orderService.GetAllOrdersAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task UpdateOrderStatusAsync_ShouldUpdateStatus_WhenOrderExists()
        {
            var order = new DomainLayer.Models.Order { Id = 1, Status = OrderStatus.Pending, Customer = new Customer { Email = "ahmed.elbakry.ab@gmail.com", Name = "AhmedBakry" } };
            var newStatus = "Shipped";

            _mockUnitOfWork.Setup(u => u.GetRepository<DomainLayer.Models.Order, int>().GetByIdAsync(It.IsAny<OrderByIdSpecification>())).ReturnsAsync(order);

            await _orderService.UpdateOrderStatusAsync(1, newStatus);

            Assert.Equal(OrderStatus.Shipped, order.Status);
            _mockMailService.Verify(m => m.SendAsync(It.IsAny<Email>()), Times.Once);
        }

        [Fact]
        public async Task UpdateOrderStatusAsync_ShouldThrowOrderNotFoundException_WhenOrderDoesNotExist()
        {
            _mockUnitOfWork.Setup(u => u.GetRepository<DomainLayer.Models.Order, int>().GetByIdAsync(It.IsAny<OrderByIdSpecification>())).ReturnsAsync((DomainLayer.Models.Order)null);

            await Assert.ThrowsAsync<OrderNotFoundException>(() => _orderService.UpdateOrderStatusAsync(999, "Shipped"));
        }

        [Fact]
        public async Task UpdateOrderStatusAsync_ShouldThrowInvalidOrderStatusException_WhenInvalidStatusProvided()
        {
            var order = new DomainLayer.Models.Order { Id = 1, Status = OrderStatus.Pending };

            _mockUnitOfWork.Setup(u => u.GetRepository<DomainLayer.Models.Order, int>().GetByIdAsync(It.IsAny<OrderByIdSpecification>())).ReturnsAsync(order);

            await Assert.ThrowsAsync<InvalidOrderStatusException>(() => _orderService.UpdateOrderStatusAsync(1, "InvalidStatus"));
        }
    }
}
