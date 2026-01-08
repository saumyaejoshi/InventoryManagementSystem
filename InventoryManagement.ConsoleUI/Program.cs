using InventoryManagement.ConsoleUI.Models;
using InventoryManagement.ConsoleUI.Services;

var api = new ProductApiService();

while (true)
{
    Console.Clear();
    Console.WriteLine("=== INVENTORY MANAGEMENT SYSTEM ===");
    Console.WriteLine("1. Add Product");
    Console.WriteLine("2. View All Products");
    Console.WriteLine("3. View Product by ID");
    Console.WriteLine("4. Update Product");
    Console.WriteLine("5. Delete Product");
    Console.WriteLine("6. Search Products");
    Console.WriteLine("7. Exit");
    Console.Write("Select option: ");

    var choice = Console.ReadLine();

    try
    {
        switch (choice)
        {
            case "1":
                await AddProduct(api);
                break;

            case "2":
                await ViewAll(api);
                break;

            case "3":
                await ViewById(api);
                break;

            case "4":
                await Update(api);
                break;

            case "5":
                await Delete(api);
                break;

            case "6":
                await Search(api);
                break;

            case "7":
                return;

            default:
                Console.WriteLine("Invalid choice");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }

    Console.WriteLine("\nPress any key to continue...");
    Console.ReadKey();
}

static async Task AddProduct(ProductApiService api)
{
    var product = new Product();

    Console.Write("Name: ");
    product.Name = Console.ReadLine()!;

    Console.Write("Category: ");
    product.Category = Console.ReadLine()!;

    Console.Write("Price: ");
    product.Price = decimal.Parse(Console.ReadLine()!);

    Console.Write("Quantity: ");
    product.Quantity = int.Parse(Console.ReadLine()!);

    await api.AddAsync(product);
    Console.WriteLine("Product added successfully.");
}

static async Task ViewAll(ProductApiService api)
{
    var products = await api.GetAllAsync();

    foreach (var p in products)
    {
        Console.WriteLine($"{p.ProductId} | {p.Name} | {p.Category} | {p.Price} | {p.Quantity}");
    }
}

static async Task ViewById(ProductApiService api)
{
    Console.Write("Enter Product ID: ");
    int id = int.Parse(Console.ReadLine()!);

    var product = await api.GetByIdAsync(id);

    if (product == null)
    {
        Console.WriteLine("Product not found.");
        return;
    }

    Console.WriteLine($"{product.ProductId} | {product.Name} | {product.Category} | {product.Price} | {product.Quantity}");
}

static async Task Update(ProductApiService api)
{
    Console.Write("Product ID: ");
    int id = int.Parse(Console.ReadLine()!);

    var product = new Product { ProductId = id };

    Console.Write("Name: ");
    product.Name = Console.ReadLine()!;

    Console.Write("Category: ");
    product.Category = Console.ReadLine()!;

    Console.Write("Price: ");
    product.Price = decimal.Parse(Console.ReadLine()!);

    Console.Write("Quantity: ");
    product.Quantity = int.Parse(Console.ReadLine()!);

    await api.UpdateAsync(product);
    Console.WriteLine("Product updated.");
}

static async Task Delete(ProductApiService api)
{
    Console.Write("Product ID: ");
    int id = int.Parse(Console.ReadLine()!);

    await api.DeleteAsync(id);
    Console.WriteLine("Product deleted.");
}

static async Task Search(ProductApiService api)
{
    Console.Write("Name: ");
    string name = Console.ReadLine()!;

    Console.Write("Category: ");
    string category = Console.ReadLine()!;

    var products = await api.SearchAsync(name, category);

    foreach (var p in products)
    {
        Console.WriteLine($"{p.ProductId} | {p.Name} | {p.Category} | {p.Price} | {p.Quantity}");
    }
}
