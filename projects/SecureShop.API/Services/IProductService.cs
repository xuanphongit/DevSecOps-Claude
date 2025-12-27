using SecureShop.API.Models;

namespace SecureShop.API.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<Product?> GetProductByIdAsync(int id);
    Task<Product> CreateProductAsync(CreateProductRequest request);
    Task<bool> DeleteProductAsync(int id);
}

