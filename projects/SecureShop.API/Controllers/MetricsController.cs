using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace SecureShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MetricsController : ControllerBase
{
    private readonly ILogger<MetricsController> _logger;

    public MetricsController(ILogger<MetricsController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Get basic application metrics
    /// </summary>
    /// <returns>Application metrics</returns>
    /// <response code="200">Returns application metrics</response>
    [HttpGet]
    [ProducesResponseType(typeof(ApplicationMetrics), StatusCodes.Status200OK)]
    public IActionResult GetMetrics()
    {
        _logger.LogInformation("GET /api/metrics - Metrics requested");

        var process = Process.GetCurrentProcess();
        var metrics = new ApplicationMetrics
        {
            Uptime = DateTime.UtcNow - process.StartTime.ToUniversalTime(),
            MemoryUsage = process.WorkingSet64,
            ThreadCount = process.Threads.Count,
            CpuTime = process.TotalProcessorTime,
            Timestamp = DateTime.UtcNow
        };

        return Ok(metrics);
    }
}

public class ApplicationMetrics
{
    public TimeSpan Uptime { get; set; }
    public long MemoryUsage { get; set; }
    public int ThreadCount { get; set; }
    public TimeSpan CpuTime { get; set; }
    public DateTime Timestamp { get; set; }
}
