


using Microsoft.AspNetCore.Authorization;

namespace Presentation.Controllers
{
    public class ProductsController(IServiceManager _serviceManager) : ApiBaseController
    {
        // GET: BaseUrl/api/Products
        [HttpGet]
        public async Task<IActionResult> GetAllProductsAsync()
        {
            var products = await _serviceManager.productService.GetAllProductsAsync();
            return Ok(products);
        }

        // GET: api/Products/{productId}
        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProductByIdAsync(int productId)
        {
            var product = await _serviceManager.productService.GetProductByIdAsync(productId);
            return Ok(product);
 
        }

        // POST: BaseUrl/api/Products
        [HttpPost]
        [Authorize(Roles = "Admin")] // Admin-only
        public async Task<IActionResult> CreateProductAsync([FromBody] CreateProductDto createProductDto)
        {
             var product = await _serviceManager.productService.CreateProductAsync(createProductDto);
            return Ok(product);

        }

        // PUT: BasUrl/api/Products/{productId}
        [HttpPut("{productId}")]
        [Authorize(Roles = "Admin")] // Admin-only
        public async Task<IActionResult> UpdateProductAsync(int productId, [FromBody] CreateProductDto updateProductDto)
        {
             var updatedProduct = await _serviceManager.productService.UpdateProductAsync(productId, updateProductDto);
             return Ok(updatedProduct);
        }

    }

}
