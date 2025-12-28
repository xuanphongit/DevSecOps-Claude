# CodeQL Vulnerabilities Analysis

## 📋 Các Vulnerabilities Đã Thêm Vào Code

### 1. **Hardcoded API Key** (Line 16)
```csharp
private const string API_KEY = "api_key_example_12345_NOT_REAL_SECRET_FOR_TESTING";
```
**Loại**: Hardcoded Secret  
**Mức độ**: HIGH  
**Vị trí**: `VulnerableService.cs:16`

### 2. **Weak Password Validation** (Line 19-22)
```csharp
public bool ValidatePassword(string password)
{
    return password.Length >= 6; // Only checks length
}
```
**Loại**: Weak Security Control  
**Mức độ**: MEDIUM  
**Vị trí**: `VulnerableService.cs:19-22`

### 3. **SQL Injection** (Line 26-35)
```csharp
public string GetUserData(string userId)
{
    using var connection = new SqlConnection(_connectionString);
    connection.Open();
    var query = $"SELECT * FROM Users WHERE Id = '{userId}'"; // String interpolation
    using var command = new SqlCommand(query, connection);
    return command.ExecuteScalar()?.ToString() ?? string.Empty;
}
```
**Loại**: SQL Injection  
**Mức độ**: CRITICAL  
**Vị trí**: `VulnerableService.cs:26-35`

### 4. **Missing Input Validation** (Line 39-42)
```csharp
public string ProcessUserInput(string userInput)
{
    return $"Processed: {userInput}"; // No validation
}
```
**Loại**: Missing Input Validation  
**Mức độ**: MEDIUM  
**Vị trí**: `VulnerableService.cs:39-42`

### 5. **Insecure Random Generation** (Line 46-50)
```csharp
public string GenerateToken()
{
    var random = new Random(); // Should use RandomNumberGenerator
    return random.Next().ToString();
}
```
**Loại**: Insecure Random  
**Mức độ**: MEDIUM  
**Vị trí**: `VulnerableService.cs:46-50`

### 6. **Hardcoded Credentials** (Line 56-57)
```csharp
private readonly string _dbPassword = "example_password_123_NOT_REAL";
private readonly string _connectionString = "Server=localhost;Database=SecureShop;User=sa;Password=ExamplePassword123!";
```
**Loại**: Hardcoded Secret  
**Mức độ**: HIGH  
**Vị trí**: `VulnerableService.cs:56-57`

---

## ❌ Tại Sao CodeQL Không Detect Được?

### 1. **SQL Injection - Không Detect**

**Lý do**:
- CodeQL C# query `cs/sql-injection` tìm pattern: user input → string concatenation → SQL execution
- Code hiện tại: `userId` (user input) → string interpolation → `SqlCommand(query)` → `ExecuteScalar()`
- **Vấn đề**: CodeQL có thể không track được data flow từ `userId` parameter qua string interpolation vào `SqlCommand`

**Giải pháp**:
```csharp
// Pattern CodeQL detect được:
var query = "SELECT * FROM Users WHERE Id = '" + userId + "'"; // String concatenation
using var command = new SqlCommand(query, connection);

// Hoặc dùng StringBuilder:
var query = new StringBuilder("SELECT * FROM Users WHERE Id = '");
query.Append(userId);
query.Append("'");
```

### 2. **Hardcoded Secrets - Không Detect**

**Lý do**:
- CodeQL secret detection dựa vào patterns cụ thể (API keys, passwords, tokens)
- Format hiện tại: `"api_key_example_12345_NOT_REAL_SECRET_FOR_TESTING"` không match patterns
- Có `pragma: allowlist secret` comment (nhưng CodeQL vẫn nên detect)

**Giải pháp**:
```csharp
// Patterns CodeQL detect được (nhưng sẽ bị GitHub Push Protection chặn):
// private const string API_KEY = "sk_live_XXXXX"; // Stripe pattern
// private const string AWS_KEY = "AKIAXXXXX"; // AWS pattern

// Alternative: Dùng format an toàn hơn (CodeQL có thể không detect):
private const string API_KEY = "api_key_TEST_12345_NOT_REAL_SECRET"; // Safe format
private const string PASSWORD = "TestPassword123!"; // Safe format
```

### 3. **Weak Password Validation - Không Detect**

**Lý do**:
- CodeQL không có built-in query cho weak password validation
- Cần custom query hoặc dùng SonarQube/SonarCloud

**Giải pháp**: Không có query mặc định, cần custom query

### 4. **Missing Input Validation - Không Detect**

**Lý do**:
- CodeQL không có generic query cho "missing validation"
- Cần context cụ thể (XSS, path traversal, etc.)

**Giải pháp**: Không có query mặc định

### 5. **Insecure Random - Có Thể Detect**

**Lý do**:
- CodeQL có query `cs/insecure-random` nhưng cần context đúng
- Pattern: `new Random()` thay vì `RandomNumberGenerator`

**Giải pháp**: Code đã đúng, có thể cần thêm context sử dụng

---

## ✅ CodeQL Alerts Hiện Tại

CodeQL đã detect được:
1. ✅ **Log Forging** (2 alerts) - `cs/log-forging`
2. ✅ **Insecure Direct Object Reference** - `cs/web/insecure-direct-object-reference`
3. ✅ **Missing Function-Level Access Control** - `cs/web/missing-function-level-access-control`

**Không detect được từ VulnerableService**:
- ❌ SQL Injection
- ❌ Hardcoded Secrets
- ❌ Weak Password Validation
- ❌ Missing Input Validation
- ❌ Insecure Random

---

## 🔧 Giải Pháp Để CodeQL Detect

### Option 1: Sửa Code Pattern

1. **SQL Injection**: Dùng string concatenation thay vì interpolation
2. **Hardcoded Secrets**: Dùng patterns CodeQL nhận diện được
3. **Insecure Random**: Đảm bảo có context sử dụng

### Option 2: Sử dụng Built-in CodeQL Queries (Best Practice)

**Không nên tạo custom queries** cho các patterns phổ biến như SQL injection và hardcoded secrets vì:
- CodeQL đã có built-in queries: `cs/sql-injection`, `cs/hardcoded-credential`
- Built-in queries được maintain bởi GitHub Security Lab, tested và optimized
- Custom queries chỉ nên dùng cho patterns **rất cụ thể** của project, không generic
- Custom queries khó maintain và có thể có false positives/negatives

**Best Practice**:
- ✅ Dùng `+security-and-quality,security-extended` (built-in queries)
- ✅ Nếu cần, dùng queries từ [CodeQL Community Packs](https://github.com/github/codeql/tree/main/csharp/ql/src/Security/CWE)
- ❌ Tránh tự viết custom queries trừ khi thực sự cần cho patterns rất cụ thể

### Option 3: Sử dụng SonarQube/SonarCloud

SonarCloud có nhiều rules hơn cho các patterns này và có thể bổ sung cho CodeQL

---

## 📊 Kết Luận

**Vulnerabilities đã thêm**: 6 loại  
**CodeQL detect được**: 0 từ VulnerableService  
**Lý do chính**: 
- SQL Injection: Pattern không match (string interpolation vs concatenation)
- Hardcoded Secrets: Format không match CodeQL patterns
- Weak/Missing Validation: Không có built-in queries
- Insecure Random: Có thể cần thêm context

**Next Steps**:
1. Sửa SQL Injection pattern (dùng string concatenation)
2. Sửa hardcoded secrets format (dùng patterns CodeQL nhận diện)
3. Thêm custom CodeQL queries nếu cần
4. Hoặc chấp nhận rằng một số vulnerabilities không được CodeQL detect (dùng SonarCloud)
