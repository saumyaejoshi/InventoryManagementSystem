using InventoryManagement.Entities;
using InventoryManagement.Repositories.Interfaces;
using InventoryManagement.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly ILogger<ProductService> _logger;
    public ProductService(
     IProductRepository repository,
     ILogger<ProductService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task AddProductAsync(Product product)
    {
        _logger.LogInformation(
            "Adding product: Name={Name}, Category={Category}",
            product.Name, product.Category);

        product.IsActive = true;
        product.CreatedDate = DateTime.UtcNow;

        await _repository.AddAsync(product);
    }


    public Task<List<Product>> GetAllProductsAsync()
    {
        _logger.LogInformation("Fetching all active products");
        return _repository.GetAllAsync();
    }


    public Task<Product?> GetProductByIdAsync(int id)
        => _repository.GetByIdAsync(id);

    public async Task UpdateProductAsync(Product product)
    {
        _logger.LogInformation(
            "Updating product: ProductId={Id}",
            product.ProductId);

        await _repository.UpdateAsync(product);
    }


    public async Task DeleteProductAsync(int id)
    {
        _logger.LogInformation(
            "Deleting product (soft): ProductId={Id}", id);

        await _repository.SoftDeleteAsync(id);
    }


    public async Task<List<Product>> SearchProductsAsync(string name, string category)
    {
        _logger.LogInformation(
            "Searching products: Name={Name}, Category={Category}",
            name, category);

        return await _repository.SearchAsync(name, category);
    }

}
