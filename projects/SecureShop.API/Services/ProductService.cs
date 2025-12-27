using System.Text.Encodings.Web;
using SecureShop.API.Models;

namespace SecureShop.API.Services;

public class ProductService : IProductService
{
    private readonly List<Product> _products;
    private readonly ILogger<ProductService> _logger;
    private int _nextId = 1;

    public ProductService(ILogger<ProductService> logger)
    {
        _logger = logger;
        _products = new List<Product>
        {
            new Product
            {
                Id = _nextId++,
                Name = "Laptop Pro",
                Description = "High-performance laptop for professionals",
                Price = 1299.99m,
                Stock = 15,
                Category = "Electronics",
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Id = _nextId++,
                Name = "Wireless Mouse",
                Description = "Ergonomic wireless mouse",
                Price = 29.99m,
                Stock = 50,
                Category = "Accessories",
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Id = _nextId++,
                Name = "Mechanical Keyboard",
                Description = "RGB mechanical keyboard",
                Price = 149.99m,
                Stock = 30,
                Category = "Accessories",
                CreatedAt = DateTime.UtcNow
            }
        };
    }

    public Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        _logger.LogInformation("Retrieving all products. Total: {Count}", _products.Count);
        return Task.FromResult<IEnumerable<Product>>(_products);
    }

    public Task<Product?> GetProductByIdAsync(int id)
    {
        _logger.LogInformation("Retrieving product with ID: {ProductId}", id);
        var product = _products.FirstOrDefault(p => p.Id == id);

        if (product == null)
        {
            _logger.LogWarning("Product with ID {ProductId} not found", id);
        }

        return Task.FromResult(product);
    }

    public Task<Product> CreateProductAsync(CreateProductRequest request)
    {
        // Security: Sanitize user input before logging to prevent log injection
        var sanitizedName = HtmlEncoder.Default.Encode(request.Name);
        _logger.LogInformation("Creating new product: {ProductName}", sanitizedName);

        var product = new Product
        {
            Id = _nextId++,
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Stock = request.Stock,
            Category = request.Category,
            CreatedAt = DateTime.UtcNow
        };

        _products.Add(product);
        _logger.LogInformation("Product created successfully with ID: {ProductId}", product.Id);

        return Task.FromResult(product);
    }

    public Task<bool> DeleteProductAsync(int id)
    {
        _logger.LogInformation("Attempting to delete product with ID: {ProductId}", id);

        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            _logger.LogWarning("Product with ID {ProductId} not found for deletion", id);
            return Task.FromResult(false);
        }

        _products.Remove(product);
        _logger.LogInformation("Product with ID {ProductId} deleted successfully", id);

        return Task.FromResult(true);
    }
}
