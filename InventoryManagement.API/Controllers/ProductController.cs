using InventoryManagement.Entities;
using InventoryManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.API.Controllers;

[ApiController]
[Route("products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;

    public ProductsController(IProductService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Add(Product product)
    {
        await _service.AddProductAsync(product);
        return Ok("Product added successfully");
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetAllProductsAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _service.GetProductByIdAsync(id);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpPut]
    public async Task<IActionResult> Update(Product product)
    {
        await _service.UpdateProductAsync(product);
        return Ok("Product updated");
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteProductAsync(id);
        return Ok("Product deleted");
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search(string name, string category)
        => Ok(await _service.SearchProductsAsync(name, category));
}
