using Microsoft.AspNetCore.Mvc;
using SecureShop.API.Services;

namespace SecureShop.API.Controllers;

/// <summary>
/// Test controller for security scanning demonstration
/// DO NOT USE IN PRODUCTION - This is for educational purposes only
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class SecurityTestController : ControllerBase
{
    private readonly VulnerableService _vulnerableService;
    private readonly ILogger<SecurityTestController> _logger;

    public SecurityTestController(VulnerableService vulnerableService, ILogger<SecurityTestController> logger)
    {
        _vulnerableService = vulnerableService;
        _logger = logger;
    }

    /// <summary>
    /// Test endpoint for SQL injection vulnerability detection
    /// </summary>
    [HttpGet("user/{userId}")]
    public IActionResult GetUserData(string userId)
    {
        // VULNERABLE: Direct use of user input in SQL query
        var query = _vulnerableService.GetUserData(userId);
        return Ok(new { query });
    }

    /// <summary>
    /// Test endpoint for weak password validation
    /// </summary>
    [HttpPost("validate-password")]
    public IActionResult ValidatePassword([FromBody] string password)
    {
        // VULNERABLE: Weak password validation
        var isValid = _vulnerableService.ValidatePassword(password);
        return Ok(new { isValid });
    }

    /// <summary>
    /// Test endpoint for missing input validation
    /// </summary>
    [HttpPost("process-input")]
    public IActionResult ProcessInput([FromBody] string userInput)
    {
        // VULNERABLE: No input validation
        var result = _vulnerableService.ProcessUserInput(userInput);
        return Ok(new { result });
    }

    /// <summary>
    /// Test endpoint for insecure random generation
    /// </summary>
    [HttpGet("generate-token")]
    public IActionResult GenerateToken()
    {
        // VULNERABLE: Insecure random generation
        var token = _vulnerableService.GenerateToken();
        return Ok(new { token });
    }
}
