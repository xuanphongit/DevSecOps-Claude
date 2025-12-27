using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SecureShop.API.Controllers;

namespace SecureShop.Tests;

public class HealthControllerTests
{
    private readonly Mock<ILogger<HealthController>> _loggerMock;
    private readonly HealthController _controller;

    public HealthControllerTests()
    {
        _loggerMock = new Mock<ILogger<HealthController>>();
        _controller = new HealthController(_loggerMock.Object);
    }

    [Fact]
    public void GetHealth_ShouldReturnOkResultWithHealthStatus()
    {
        // Act
        var result = _controller.GetHealth();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeOfType<HealthStatus>();

        var healthStatus = okResult.Value as HealthStatus;
        healthStatus!.Status.Should().Be("Healthy");
        healthStatus.Version.Should().NotBeNullOrEmpty();
    }
}
