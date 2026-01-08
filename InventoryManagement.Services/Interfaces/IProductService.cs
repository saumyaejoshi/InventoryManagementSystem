using InventoryManagement.Entities;

namespace InventoryManagement.Services.Interfaces;

public interface IProductService
{
    Task AddProductAsync(Product product);
    Task<List<Product>> GetAllProductsAsync();
    Task<Product?> GetProductByIdAsync(int id);
    Task UpdateProductAsync(Product product);
    Task DeleteProductAsync(int id);
    Task<List<Product>> SearchProductsAsync(string name, string category);
}
