


namespace ServiceLayer.Tests
{
    public class ProductServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _productService = new ProductService(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllProductsAsync_ShouldReturnListOfProducts()
        {
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product A", Price = 100m, Stock = 10 },
                new Product { Id = 2, Name = "Product B", Price = 200m, Stock = 20 }
            };

            _mockUnitOfWork.Setup(u => u.GetRepository<Product, int>().GetAllAsync()).ReturnsAsync(products);
            _mockMapper.Setup(m => m.Map<List<ProductDto>>(products)).Returns(new List<ProductDto>
            {
                new ProductDto { Id = 1, Name = "Product A", Price = 100m, Stock = 10 },
                new ProductDto { Id = 2, Name = "Product B", Price = 200m, Stock = 20 }
            });

            var result = await _productService.GetAllProductsAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Product A", result[0].Name);
            Assert.Equal(100m, result[0].Price);
            _mockUnitOfWork.Verify(u => u.GetRepository<Product, int>().GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetProductByIdAsync_ShouldReturnProduct_WhenProductExists()
        {
            var productId = 1;
            var product = new Product { Id = productId, Name = "Product A", Price = 100m, Stock = 10 };

            _mockUnitOfWork.Setup(u => u.GetRepository<Product, int>().GetByIdAsync(productId)).ReturnsAsync(product);
            _mockMapper.Setup(m => m.Map<ProductDto>(product)).Returns(new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock
            });

            var result = await _productService.GetProductByIdAsync(productId);

            Assert.NotNull(result);
            Assert.Equal(productId, result.Id);
            Assert.Equal("Product A", result.Name);
            _mockUnitOfWork.Verify(u => u.GetRepository<Product, int>().GetByIdAsync(productId), Times.Once);
        }

        [Fact]
        public async Task GetProductByIdAsync_ShouldThrowProductNotFoundException_WhenProductDoesNotExist()
        {
            var productId = 1;
            _mockUnitOfWork.Setup(u => u.GetRepository<Product, int>().GetByIdAsync(productId)).ReturnsAsync((Product)null);

            await Assert.ThrowsAsync<ProductNotFoundException>(() => _productService.GetProductByIdAsync(productId));
        }

        [Fact]
        public async Task CreateProductAsync_ShouldCreateProductSuccessfully()
        {
            var createProductDto = new CreateProductDto
            {
                Name = "Product A",
                Price = 100m,
                Stock = 10
            };

            var product = new Product
            {
                Id = 1,
                Name = createProductDto.Name,
                Price = createProductDto.Price,
                Stock = createProductDto.Stock
            };

            _mockMapper.Setup(m => m.Map<Product>(createProductDto)).Returns(product);
            _mockUnitOfWork.Setup(u => u.GetRepository<Product, int>().AddAsync(product)).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);
            _mockMapper.Setup(m => m.Map<ProductDto>(product)).Returns(new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock
            });

            var result = await _productService.CreateProductAsync(createProductDto);

            Assert.NotNull(result);
            Assert.Equal(createProductDto.Name, result.Name);
            _mockUnitOfWork.Verify(u => u.GetRepository<Product, int>().AddAsync(product), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateProductAsync_ShouldUpdateProductSuccessfully_WhenProductExists()
        {
            var productId = 1;
            var updateProductDto = new CreateProductDto
            {
                Name = "Updated Product",
                Price = 150m,
                Stock = 15
            };

            var product = new Product
            {
                Id = productId,
                Name = "Product A",
                Price = 100m,
                Stock = 10
            };

            _mockUnitOfWork.Setup(u => u.GetRepository<Product, int>().GetByIdAsync(productId)).ReturnsAsync(product);
            _mockMapper.Setup(m => m.Map<ProductDto>(product)).Returns(new ProductDto
            {
                Id = product.Id,
                Name = updateProductDto.Name,
                Price = updateProductDto.Price,
                Stock = updateProductDto.Stock
            });

            var result = await _productService.UpdateProductAsync(productId, updateProductDto);

            Assert.NotNull(result);
            Assert.Equal(updateProductDto.Name, result.Name);
            Assert.Equal(updateProductDto.Price, result.Price);
            Assert.Equal(updateProductDto.Stock, result.Stock);

            _mockUnitOfWork.Verify(u => u.GetRepository<Product, int>().Update(It.IsAny<Product>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateProductAsync_ShouldThrowProductNotFoundException_WhenProductDoesNotExist()
        {
            var productId = 1;
            var updateProductDto = new CreateProductDto
            {
                Name = "Updated Product",
                Price = 150m,
                Stock = 15
            };

            _mockUnitOfWork.Setup(u => u.GetRepository<Product, int>().GetByIdAsync(productId)).ReturnsAsync((Product)null);

            await Assert.ThrowsAsync<ProductNotFoundException>(() => _productService.UpdateProductAsync(productId, updateProductDto));
        }
    }
}
