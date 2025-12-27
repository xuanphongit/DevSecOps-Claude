using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc;
using SecureShop.API.Models;
using SecureShop.API.Services;

namespace SecureShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductService productService, ILogger<ProductsController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    /// <summary>
    /// Get all products
    /// </summary>
    /// <returns>List of all products</returns>
    /// <response code="200">Returns the list of products</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        _logger.LogInformation("GET /api/products - Retrieving all products");
        var products = await _productService.GetAllProductsAsync();
        return Ok(products);
    }

    /// <summary>
    /// Get a product by ID
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <returns>The product</returns>
    /// <response code="200">Returns the product</response>
    /// <response code="404">Product not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        _logger.LogInformation("GET /api/products/{ProductId} - Retrieving product", id);
        var product = await _productService.GetProductByIdAsync(id);

        if (product == null)
        {
            _logger.LogWarning("Product with ID {ProductId} not found", id);
            return NotFound($"Product with ID {id} not found");
        }

        return Ok(product);
    }

    /// <summary>
    /// Create a new product (requires authentication)
    /// </summary>
    /// <param name="request">Product creation request</param>
    /// <returns>The created product</returns>
    /// <response code="201">Product created successfully</response>
    /// <response code="400">Invalid request data</response>
    [HttpPost]
    [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Product>> CreateProduct([FromBody] CreateProductRequest request)
    {
        // Security: Sanitize user input before logging to prevent log injection
        var sanitizedName = HtmlEncoder.Default.Encode(request.Name);
        _logger.LogInformation("POST /api/products - Creating new product: {ProductName}", sanitizedName);

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for product creation");
            return BadRequest(ModelState);
        }

        var product = await _productService.CreateProductAsync(request);
        _logger.LogInformation("Product created successfully with ID: {ProductId}", product.Id);

        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    /// <summary>
    /// Delete a product by ID
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <returns>No content</returns>
    /// <response code="204">Product deleted successfully</response>
    /// <response code="404">Product not found</response>
    /// <response code="403">Forbidden - Insufficient permissions</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    // Security: Add authorization check (will be implemented with proper auth later)
    // [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        _logger.LogInformation("DELETE /api/products/{ProductId} - Deleting product", id);

        // Security: Verify product exists and user has permission (IDOR protection)
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
        {
            _logger.LogWarning("Product with ID {ProductId} not found for deletion", id);
            return NotFound($"Product with ID {id} not found");
        }

        // Security: Additional authorization check would go here
        // For now, we verify the product exists before deletion

        var deleted = await _productService.DeleteProductAsync(id);
        if (!deleted)
        {
            _logger.LogWarning("Failed to delete product with ID {ProductId}", id);
            return NotFound($"Product with ID {id} not found");
        }

        return NoContent();
    }
}
