using InventoryManagement.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryManagement.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task AddAsync(Product product);
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task UpdateAsync(Product product);
        Task SoftDeleteAsync(int id);
        Task<List<Product>> SearchAsync(string name, string category);
    }
}
