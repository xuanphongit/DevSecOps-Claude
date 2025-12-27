using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SecureShop.API.Models;
using SecureShop.API.Services;

namespace SecureShop.Tests;

public class ProductServiceTests
{
    private readonly Mock<ILogger<ProductService>> _loggerMock;
    private readonly ProductService _service;

    public ProductServiceTests()
    {
        _loggerMock = new Mock<ILogger<ProductService>>();
        _service = new ProductService(_loggerMock.Object);
    }

    [Fact]
    public async Task GetAllProductsAsync_ShouldReturnAllProducts()
    {
        // Act
        var result = await _service.GetAllProductsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCountGreaterThan(0);
    }

    [Fact]
    public async Task GetProductByIdAsync_WithValidId_ShouldReturnProduct()
    {
        // Arrange
        var products = await _service.GetAllProductsAsync();
        var firstProduct = products.First();

        // Act
        var result = await _service.GetProductByIdAsync(firstProduct.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(firstProduct.Id);
        result.Name.Should().Be(firstProduct.Name);
    }

    [Fact]
    public async Task GetProductByIdAsync_WithInvalidId_ShouldReturnNull()
    {
        // Act
        var result = await _service.GetProductByIdAsync(99999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateProductAsync_WithValidRequest_ShouldCreateProduct()
    {
        // Arrange
        var request = new CreateProductRequest
        {
            Name = "Test Product",
            Description = "Test Description",
            Price = 99.99m,
            Stock = 10,
            Category = "Test Category"
        };

        // Act
        var result = await _service.CreateProductAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.Name.Should().Be(request.Name);
        result.Description.Should().Be(request.Description);
        result.Price.Should().Be(request.Price);
        result.Stock.Should().Be(request.Stock);
        result.Category.Should().Be(request.Category);
        result.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task DeleteProductAsync_WithValidId_ShouldReturnTrue()
    {
        // Arrange
        var request = new CreateProductRequest
        {
            Name = "Product to Delete",
            Description = "Will be deleted",
            Price = 50.00m,
            Stock = 5,
            Category = "Test"
        };
        var product = await _service.CreateProductAsync(request);

        // Act
        var result = await _service.DeleteProductAsync(product.Id);

        // Assert
        result.Should().BeTrue();
        
        // Verify product is deleted
        var deletedProduct = await _service.GetProductByIdAsync(product.Id);
        deletedProduct.Should().BeNull();
    }

    [Fact]
    public async Task DeleteProductAsync_WithInvalidId_ShouldReturnFalse()
    {
        // Act
        var result = await _service.DeleteProductAsync(99999);

        // Assert
        result.Should().BeFalse();
    }
}

