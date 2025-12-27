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
    // CodeQL should detect this hardcoded secret (pragma only suppresses pre-commit hook)
    private const string API_KEY = "api_key_example_12345_not_real_secret"; // pragma: allowlist secret

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
        var query = $"SELECT * FROM Users WHERE Id = '{userId}'";
        // In real scenario, this would execute against database
        return query;
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
    // CodeQL should detect these hardcoded secrets (pragma only suppresses pre-commit hook)
    private readonly string _dbPassword = "admin123"; // pragma: allowlist secret
    private readonly string _connectionString = "Server=localhost;Database=SecureShop;User=sa;Password=Password123!"; // pragma: allowlist secret
}
