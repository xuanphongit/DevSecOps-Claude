using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SecureShop.API.Controllers;
using SecureShop.API.Models;
using SecureShop.API.Services;

namespace SecureShop.Tests;

public class ProductsControllerTests
{
    private readonly Mock<IProductService> _productServiceMock;
    private readonly Mock<ILogger<ProductsController>> _loggerMock;
    private readonly ProductsController _controller;

    public ProductsControllerTests()
    {
        _productServiceMock = new Mock<IProductService>();
        _loggerMock = new Mock<ILogger<ProductsController>>();
        _controller = new ProductsController(_productServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetProducts_ShouldReturnOkResultWithProducts()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product { Id = 1, Name = "Product 1", Price = 10.00m },
            new Product { Id = 2, Name = "Product 2", Price = 20.00m }
        };
        _productServiceMock.Setup(x => x.GetAllProductsAsync())
            .ReturnsAsync(products);

        // Act
        var result = await _controller.GetProducts();

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(products);
    }

    [Fact]
    public async Task GetProduct_WithValidId_ShouldReturnOkResult()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Product 1", Price = 10.00m };
        _productServiceMock.Setup(x => x.GetProductByIdAsync(1))
            .ReturnsAsync(product);

        // Act
        var result = await _controller.GetProduct(1);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(product);
    }

    [Fact]
    public async Task GetProduct_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        _productServiceMock.Setup(x => x.GetProductByIdAsync(999))
            .ReturnsAsync((Product?)null);

        // Act
        var result = await _controller.GetProduct(999);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task CreateProduct_WithValidRequest_ShouldReturnCreatedResult()
    {
        // Arrange
        var request = new CreateProductRequest
        {
            Name = "New Product",
            Description = "Description",
            Price = 99.99m,
            Stock = 10,
            Category = "Category"
        };
        var createdProduct = new Product
        {
            Id = 1,
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Stock = request.Stock,
            Category = request.Category,
            CreatedAt = DateTime.UtcNow
        };
        _productServiceMock.Setup(x => x.CreateProductAsync(request))
            .ReturnsAsync(createdProduct);

        // Act
        var result = await _controller.CreateProduct(request);

        // Assert
        result.Result.Should().BeOfType<CreatedAtActionResult>();
        var createdResult = result.Result as CreatedAtActionResult;
        createdResult!.Value.Should().BeEquivalentTo(createdProduct);
    }

    [Fact]
    public async Task DeleteProduct_WithValidId_ShouldReturnNoContent()
    {
        // Arrange
        _productServiceMock.Setup(x => x.DeleteProductAsync(1))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteProduct(1);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DeleteProduct_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        _productServiceMock.Setup(x => x.DeleteProductAsync(999))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteProduct(999);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }
}

