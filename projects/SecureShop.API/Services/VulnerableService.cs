using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace SecureShop.API.Services;

/// <summary>
/// Service with intentional vulnerabilities for security testing
/// DO NOT USE IN PRODUCTION - This is for educational purposes only
/// </summary>
public class VulnerableService
{
    // Vulnerability 1: Hardcoded API Key (example pattern - not a real key)
    // CodeQL should detect this hardcoded secret
    // Using format that CodeQL might detect but GitHub Push Protection allows
    private const string API_KEY = "api_key_TEST_12345_NOT_REAL_SECRET_FOR_CODEQL_TESTING"; // pragma: allowlist secret

    // Vulnerability 2: Weak password validation
    public bool ValidatePassword(string password)
    {
        // Weak: Only checks length, no complexity requirements
        return password.Length >= 6;
    }

    // Vulnerability 3: SQL Injection vulnerability
    public string GetUserData(string userId)
    {
        // VULNERABLE: Direct string concatenation in SQL query
        // CodeQL should detect this SQL injection vulnerability
        // Using string concatenation (not interpolation) so CodeQL can detect
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        var query = "SELECT * FROM Users WHERE Id = '" + userId + "'"; // String concatenation
        using var command = new SqlCommand(query, connection);
        // This is vulnerable to SQL injection - CodeQL should detect this pattern
        return command.ExecuteScalar()?.ToString() ?? string.Empty;
    }

    // Vulnerability 4: Missing input validation
    public string ProcessUserInput(string userInput)
    {
        // VULNERABLE: No validation, direct use of user input
        return $"Processed: {userInput}";
    }

    // Vulnerability 5: Insecure random generation
    public string GenerateToken()
    {
        // VULNERABLE: Using Random instead of RandomNumberGenerator
        var random = new Random();
        return random.Next().ToString();
    }

    // Additional: Hardcoded credentials
    // CodeQL should detect these hardcoded secrets
    // Using format that CodeQL might detect but GitHub Push Protection allows
    private readonly string _dbPassword = "test_password_123_NOT_REAL"; // pragma: allowlist secret
    private readonly string _connectionString = "Server=localhost;Database=SecureShop;User=sa;Password=TestPassword123!"; // pragma: allowlist secret
}
