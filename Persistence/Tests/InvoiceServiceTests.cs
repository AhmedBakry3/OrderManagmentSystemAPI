


namespace ServiceLayer.Tests
{
    public class InvoiceServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly InvoiceService _invoiceService;

        public InvoiceServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _invoiceService = new InvoiceService(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetInvoiceByIdAsync_ShouldReturnInvoice_WhenInvoiceExists()
        {
            var invoiceId = 1;
            var invoice = new Invoice
            {
                Id = invoiceId,
                OrderId = 123,
                InvoiceDate = DateTime.UtcNow,
                TotalAmount = 250.50m
            };

            _mockUnitOfWork.Setup(u => u.GetRepository<Invoice, int>().GetByIdAsync(It.IsAny<InvoiceWithOrderSpecification>())).ReturnsAsync(invoice);
            _mockMapper.Setup(m => m.Map<InvoiceDto>(invoice)).Returns(new InvoiceDto
            {
                Id = invoice.Id,
                OrderId = invoice.OrderId,
                InvoiceDate = invoice.InvoiceDate,
                TotalAmount = invoice.TotalAmount
            });

            var result = await _invoiceService.GetInvoiceByIdAsync(invoiceId);

            Assert.NotNull(result);
            Assert.Equal(invoiceId, result.Id);
            Assert.Equal(invoice.OrderId, result.OrderId);
            Assert.Equal(invoice.InvoiceDate, result.InvoiceDate);
            Assert.Equal(invoice.TotalAmount, result.TotalAmount);
            _mockUnitOfWork.Verify(u => u.GetRepository<Invoice, int>().GetByIdAsync(It.IsAny<InvoiceWithOrderSpecification>()), Times.Once);
        }

        [Fact]
        public async Task GetInvoiceByIdAsync_ShouldThrowInvoiceNotFoundException_WhenInvoiceDoesNotExist()
        {
            var invoiceId = 1;

            _mockUnitOfWork.Setup(u => u.GetRepository<Invoice, int>().GetByIdAsync(It.IsAny<InvoiceWithOrderSpecification>())).ReturnsAsync((Invoice)null);

            await Assert.ThrowsAsync<InvoiceNotFoundException>(() => _invoiceService.GetInvoiceByIdAsync(invoiceId));
        }

        [Fact]
        public async Task GetAllInvoicesAsync_ShouldReturnListOfInvoices()
        {
            var invoices = new List<Invoice>
            {
                new Invoice { Id = 1, OrderId = 123, InvoiceDate = DateTime.UtcNow, TotalAmount = 250.50m },
                new Invoice { Id = 2, OrderId = 124, InvoiceDate = DateTime.UtcNow, TotalAmount = 320.00m }
            };

            _mockUnitOfWork.Setup(u => u.GetRepository<Invoice, int>().GetAllAsync(It.IsAny<InvoiceWithOrderSpecification>())).ReturnsAsync(invoices);
            _mockMapper.Setup(m => m.Map<IEnumerable<InvoiceDto>>(invoices)).Returns(new List<InvoiceDto>
            {
                new InvoiceDto { Id = 1, OrderId = 123, InvoiceDate = DateTime.UtcNow, TotalAmount = 250.50m },
                new InvoiceDto { Id = 2, OrderId = 124, InvoiceDate = DateTime.UtcNow, TotalAmount = 320.00m }
            });

            var result = await _invoiceService.GetAllInvoicesAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _mockUnitOfWork.Verify(u => u.GetRepository<Invoice, int>().GetAllAsync(It.IsAny<InvoiceWithOrderSpecification>()), Times.Once);
        }
    }
}
