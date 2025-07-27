



namespace ServiceLayer.Tests
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IMailService mailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mailService = mailService;
        }

        public async Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            var customer = await _unitOfWork.GetRepository<Customer, int>()
                .GetByIdAsync(createOrderDto.CustomerId);

            if (customer == null)
                throw new CustomerNotFoundException(createOrderDto.CustomerId);

            decimal totalAmount = 0;
            var orderItems = new List<OrderItem>();

            foreach (var itemDto in createOrderDto.OrderItems)
            {
                var orderItem = new OrderItem
                {
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity,
                    UnitPrice = itemDto.UnitPrice,
                    Discount = itemDto.Discount
                };

                totalAmount += itemDto.Quantity * itemDto.UnitPrice;
                orderItems.Add(orderItem);
            }

            if (totalAmount > 200)
                totalAmount *= 0.90m;
            else if (totalAmount > 100)
                totalAmount *= 0.95m;

            var order = new DomainLayer.Models.Order
            {
                CustomerId = createOrderDto.CustomerId,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                TotalAmount = totalAmount,
                OrderItems = orderItems,
                PaymentMethod = createOrderDto.PaymentMethod
            };

            foreach (var item in orderItems)
            {
                var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(item.ProductId);
                if (product == null)
                    throw new ProductNotFoundException(item.ProductId);

                if (product.Stock < item.Quantity)
                    throw new InsufficientStockException(item.ProductId, item.Quantity, product.Stock);

                product.Stock -= item.Quantity;
                _unitOfWork.GetRepository<Product, int>().Update(product);
            }

            await _unitOfWork.GetRepository<DomainLayer.Models.Order, int>().AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            var invoice = new Invoice
            {
                OrderId = order.Id,
                InvoiceDate = DateTime.UtcNow,
                TotalAmount = totalAmount
            };

            await _unitOfWork.GetRepository<Invoice, int>().AddAsync(invoice);
            await _unitOfWork.SaveChangesAsync();

            var email = new Email
            {
                To = customer.Email,
                Subject = $"Order #{order.Id} Confirmation",
                Body = $"Dear {customer.Name},\n\nYour order #{order.Id} has been successfully placed. Total: {totalAmount:C}.\n\nThanks!"
            };

            await _mailService.SendAsync(email);

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto> GetOrderByIdAsync(int orderId)
        {
            var spec = new OrderByIdSpecification(orderId);
            var order = await _unitOfWork.GetRepository<DomainLayer.Models.Order, int>().GetByIdAsync(spec);

            if (order == null)
                throw new OrderNotFoundException(orderId);

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<List<OrderDto>> GetOrdersByCustomerIdAsync(int customerId)
        {
            var spec = new OrdersByCustomerIdSpecifications(customerId);
            var orders = await _unitOfWork.GetRepository<DomainLayer.Models.Order, int>().GetAllAsync(spec);

            return _mapper.Map<List<OrderDto>>(orders);
        }

        public async Task<List<OrderDto>> GetAllOrdersAsync()
        {
            var spec = new AllOrdersSpecifications();
            var orders = await _unitOfWork.GetRepository<DomainLayer.Models.Order, int>().GetAllAsync(spec);

            return _mapper.Map<List<OrderDto>>(orders);
        }

        public async Task UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            var spec = new OrderByIdSpecification(orderId);
            var repo = _unitOfWork.GetRepository<DomainLayer.Models.Order, int>();
            var order = await repo.GetByIdAsync(spec);

            if (order == null)
                throw new OrderNotFoundException(orderId);

            if (Enum.TryParse<OrderStatus>(newStatus, true, out var parsedStatus))
            {
                order.Status = parsedStatus;
            }
            else
            {
                throw new InvalidOrderStatusException(newStatus);
            }

            await _unitOfWork.SaveChangesAsync();

            var email = new Email
            {
                To = order.Customer.Email,
                Subject = $"Order #{order.Id} Status Updated",
                Body = $"Dear {order.Customer.Name},\n\nYour order #{order.Id} status has been updated to '{order.Status}'.\n\nThanks!"
            };

            await _mailService.SendAsync(email);
        }
    }
}
