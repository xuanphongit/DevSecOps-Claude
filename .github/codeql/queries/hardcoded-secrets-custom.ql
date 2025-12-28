/**
 * @name Hardcoded Secrets (Custom Pattern)
 * @description Detects hardcoded API keys, passwords, and other secrets in code using custom patterns
 * @id cs/custom/hardcoded-secrets
 * @kind problem
 * @problem.severity error
 * @precision medium
 * @tags security
 * @tags external/cwe/cwe-798
 */

import csharp

/**
 * Detects hardcoded secrets with common patterns in string literals
 */
class HardcodedSecretLiteral extends StringLiteral {
  HardcodedSecretLiteral() {
    // Pattern 1: API keys (api_key, API_KEY, apikey)
    this.getValue().regexpMatch("(?i).*api[_-]?key.*") or
    // Pattern 2: Passwords in connection strings
    this.getValue().regexpMatch("(?i).*password\\s*=\\s*[^;]+") or
    // Pattern 3: Tokens
    this.getValue().regexpMatch("(?i).*token.*") or
    // Pattern 4: Credentials
    this.getValue().regexpMatch("(?i).*credential.*") or
    // Pattern 5: Connection strings with passwords
    this.getValue().regexpMatch("(?i).*connectionstring.*password.*")
  }
}

from HardcodedSecretLiteral secret
select secret, "Hardcoded secret detected: $@",
  secret.getValue()
