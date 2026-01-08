using InventoryManagement.Entities;
using InventoryManagement.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace InventoryManagement.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly string _connectionString;

    public ProductRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }
    public async Task<bool> TestConnectionAsync()
    {
        await using SqlConnection conn = new(_connectionString);
        await conn.OpenAsync();
        return conn.State == System.Data.ConnectionState.Open;
    }

    public async Task AddAsync(Product product)
    {
        await using SqlConnection conn = new(_connectionString);
        await using SqlCommand cmd = new(
            """
            INSERT INTO Products (Name, Category, Price, Quantity, IsActive, CreatedDate)
            VALUES (@Name, @Category, @Price, @Quantity, 1, GETDATE())
            """, conn);

        cmd.Parameters.AddWithValue("@Name", product.Name);
        cmd.Parameters.AddWithValue("@Category", product.Category);
        cmd.Parameters.AddWithValue("@Price", product.Price);
        cmd.Parameters.AddWithValue("@Quantity", product.Quantity);

        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<List<Product>> GetAllAsync()
    {
        List<Product> products = [];

        await using SqlConnection conn = new(_connectionString);
        await using SqlCommand cmd = new(
            "SELECT * FROM Products WHERE IsActive = 1", conn);

        await conn.OpenAsync();
        await using SqlDataReader reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
            products.Add(Map(reader));

        return products;
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        await using SqlConnection conn = new(_connectionString);
        await using SqlCommand cmd = new(
            "SELECT * FROM Products WHERE ProductId=@Id AND IsActive=1", conn);

        cmd.Parameters.AddWithValue("@Id", id);

        await conn.OpenAsync();
        await using SqlDataReader reader = await cmd.ExecuteReaderAsync();

        return await reader.ReadAsync() ? Map(reader) : null;
    }

    public async Task UpdateAsync(Product product)
    {
        await using SqlConnection conn = new(_connectionString);
        await using SqlCommand cmd = new(
            """
            UPDATE Products
            SET Name=@Name, Category=@Category, Price=@Price, Quantity=@Quantity
            WHERE ProductId=@Id
            """, conn);

        cmd.Parameters.AddWithValue("@Id", product.ProductId);
        cmd.Parameters.AddWithValue("@Name", product.Name);
        cmd.Parameters.AddWithValue("@Category", product.Category);
        cmd.Parameters.AddWithValue("@Price", product.Price);
        cmd.Parameters.AddWithValue("@Quantity", product.Quantity);

        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task SoftDeleteAsync(int id)
    {
        await using SqlConnection conn = new(_connectionString);
        await using SqlCommand cmd = new(
            "UPDATE Products SET IsActive=0 WHERE ProductId=@Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);

        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<List<Product>> SearchAsync(string name, string category)
    {
        List<Product> products = [];

        await using SqlConnection conn = new(_connectionString);
        await using SqlCommand cmd = new(
            """
            SELECT * FROM Products
            WHERE IsActive=1
            AND Name LIKE '%' + @Name + '%'
            AND Category LIKE '%' + @Category + '%'
            """, conn);

        cmd.Parameters.AddWithValue("@Name", name ?? "");
        cmd.Parameters.AddWithValue("@Category", category ?? "");

        await conn.OpenAsync();
        await using SqlDataReader reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
            products.Add(Map(reader));

        return products;
    }

    private static Product Map(SqlDataReader reader) => new()
    {
        ProductId = (int)reader["ProductId"],
        Name = reader["Name"].ToString()!,
        Category = reader["Category"].ToString()!,
        Price = (decimal)reader["Price"],
        Quantity = (int)reader["Quantity"],
        IsActive = (bool)reader["IsActive"],
        CreatedDate = (DateTime)reader["CreatedDate"]
    };
}
