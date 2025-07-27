


namespace Service
{
    public class ProductService(IUnitOfWork _unitOfWork, IMapper _mapper) : IProductService
    {
        public async Task<List<ProductDto>> GetAllProductsAsync()
        {
            var productRepo = _unitOfWork.GetRepository<Product, int>();
            var products = await productRepo.GetAllAsync();
            return _mapper.Map<List<ProductDto>>(products);
        }

        public async Task<ProductDto> GetProductByIdAsync(int productId)
        {
            var productRepo = _unitOfWork.GetRepository<Product, int>();
            var product = await productRepo.GetByIdAsync(productId);

            if (product == null)
                throw new ProductNotFoundException(productId);

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto)
        {
            var product = _mapper.Map<Product>(createProductDto);
            var productRepo = _unitOfWork.GetRepository<Product, int>();

            await productRepo.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> UpdateProductAsync(int productId, CreateProductDto updateProductDto)
        {
            var productRepo = _unitOfWork.GetRepository<Product, int>();
            var product = await productRepo.GetByIdAsync(productId);

            if (product == null)
                throw new ProductNotFoundException(productId);

            // Update product properties
            product.Name = updateProductDto.Name;
            product.Price = updateProductDto.Price;
            product.Stock = updateProductDto.Stock;

            productRepo.Update(product);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ProductDto>(product);
        }
    }

}
