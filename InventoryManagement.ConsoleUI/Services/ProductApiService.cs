using InventoryManagement.ConsoleUI.Models;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace InventoryManagement.ConsoleUI.Services
{
    public class ProductApiService
    {
        private readonly HttpClient _httpClient;

        public ProductApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7004/")
            };
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Product>>("products")
                   ?? new List<Product>();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Product>($"products/{id}");
        }

        public async Task AddAsync(Product product)
        {
            var response = await _httpClient.PostAsJsonAsync("products", product);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateAsync(Product product)
        {
            var response = await _httpClient.PutAsJsonAsync("products", product);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"products/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<Product>> SearchAsync(string name, string category)
        {
            return await _httpClient.GetFromJsonAsync<List<Product>>(
                $"products/search?name={name}&category={category}")
                ?? new List<Product>();
        }
    }
}
