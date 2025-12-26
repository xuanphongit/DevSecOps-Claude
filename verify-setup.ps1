# Development Environment Verification Script
# Run this script to verify all tools are properly installed and configured

Write-Host "=== Development Environment Verification ===" -ForegroundColor Cyan
Write-Host ""

$allChecksPassed = $true

# Azure CLI
Write-Host "[1] Checking Azure CLI..." -ForegroundColor Yellow
try {
    $azVersion = az --version 2>&1 | Select-String "azure-cli"
    if ($azVersion) {
        Write-Host "   ✅ Azure CLI installed" -ForegroundColor Green
        $account = az account show 2>&1 | ConvertFrom-Json -ErrorAction SilentlyContinue
        if ($account -and $account.name) {
            Write-Host "   ✅ Azure logged in: $($account.name)" -ForegroundColor Green
            Write-Host "   ✅ Subscription ID: $($account.id)" -ForegroundColor Green
        } else {
            Write-Host "   ❌ Azure not logged in. Run: az login" -ForegroundColor Red
            $allChecksPassed = $false
        }
    } else {
        Write-Host "   ❌ Azure CLI not found" -ForegroundColor Red
        $allChecksPassed = $false
    }
} catch {
    Write-Host "   ❌ Error checking Azure CLI: $_" -ForegroundColor Red
    $allChecksPassed = $false
}

# kubectl
Write-Host "`n[2] Checking kubectl..." -ForegroundColor Yellow
try {
    $kubectlVersion = kubectl version --client --output json 2>&1 | ConvertFrom-Json -ErrorAction SilentlyContinue
    if ($kubectlVersion) {
        Write-Host "   ✅ kubectl installed: $($kubectlVersion.clientVersion.gitVersion)" -ForegroundColor Green
        $cluster = kubectl cluster-info 2>&1
        if ($LASTEXITCODE -eq 0) {
            Write-Host "   ✅ Kubernetes cluster accessible" -ForegroundColor Green
            $nodes = kubectl get nodes --no-headers 2>&1
            if ($LASTEXITCODE -eq 0) {
                Write-Host "   ✅ Kubernetes nodes available" -ForegroundColor Green
            }
        } else {
            Write-Host "   ⚠️  Kubernetes cluster not running" -ForegroundColor Yellow
            Write-Host "      → Start Docker Desktop and enable Kubernetes" -ForegroundColor Gray
        }
    } else {
        Write-Host "   ❌ kubectl not found" -ForegroundColor Red
        $allChecksPassed = $false
    }
} catch {
    Write-Host "   ❌ Error checking kubectl: $_" -ForegroundColor Red
    $allChecksPassed = $false
}

# Helm
Write-Host "`n[3] Checking Helm..." -ForegroundColor Yellow
try {
    $helmVersion = helm version --short 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "   ✅ Helm installed: $helmVersion" -ForegroundColor Green
    } else {
        Write-Host "   ❌ Helm not found" -ForegroundColor Red
        $allChecksPassed = $false
    }
} catch {
    Write-Host "   ❌ Error checking Helm: $_" -ForegroundColor Red
    $allChecksPassed = $false
}

# Docker
Write-Host "`n[4] Checking Docker..." -ForegroundColor Yellow
try {
    $dockerVersion = docker --version 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "   ✅ Docker installed: $dockerVersion" -ForegroundColor Green
        $dockerPs = docker ps 2>&1
        if ($LASTEXITCODE -eq 0) {
            Write-Host "   ✅ Docker daemon running" -ForegroundColor Green
        } else {
            Write-Host "   ⚠️  Docker daemon not running" -ForegroundColor Yellow
            Write-Host "      → Start Docker Desktop" -ForegroundColor Gray
        }
    } else {
        Write-Host "   ❌ Docker not found" -ForegroundColor Red
        $allChecksPassed = $false
    }
} catch {
    Write-Host "   ❌ Error checking Docker: $_" -ForegroundColor Red
    $allChecksPassed = $false
}

# Git
Write-Host "`n[5] Checking Git..." -ForegroundColor Yellow
try {
    $gitVersion = git --version 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "   ✅ Git installed: $gitVersion" -ForegroundColor Green
    } else {
        Write-Host "   ❌ Git not found" -ForegroundColor Red
        $allChecksPassed = $false
    }
} catch {
    Write-Host "   ❌ Error checking Git: $_" -ForegroundColor Red
    $allChecksPassed = $false
}

# .NET SDK
Write-Host "`n[6] Checking .NET SDK..." -ForegroundColor Yellow
try {
    $dotnetVersion = dotnet --version 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "   ✅ .NET SDK installed: $dotnetVersion" -ForegroundColor Green
    } else {
        Write-Host "   ⚠️  .NET SDK not found (needed for Task 1.2)" -ForegroundColor Yellow
        Write-Host "      → Install: winget install Microsoft.DotNet.SDK.8" -ForegroundColor Gray
    }
} catch {
    Write-Host "   ⚠️  .NET SDK not found (needed for Task 1.2)" -ForegroundColor Yellow
}

# Pre-commit
Write-Host "`n[7] Checking Pre-commit..." -ForegroundColor Yellow
try {
    $precommitVersion = pre-commit --version 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "   ✅ Pre-commit installed: $precommitVersion" -ForegroundColor Green
    } else {
        Write-Host "   ⚠️  Pre-commit not installed" -ForegroundColor Yellow
        Write-Host "      → Install: pip install pre-commit detect-secrets" -ForegroundColor Gray
    }
} catch {
    Write-Host "   ⚠️  Pre-commit not installed" -ForegroundColor Yellow
    Write-Host "      → Install: pip install pre-commit detect-secrets" -ForegroundColor Gray
}

# Summary
Write-Host "`n=== Verification Summary ===" -ForegroundColor Cyan
if ($allChecksPassed) {
    Write-Host "✅ Core tools are installed and configured!" -ForegroundColor Green
    Write-Host "⚠️  Note: Docker Desktop and Kubernetes need to be started manually" -ForegroundColor Yellow
} else {
    Write-Host "❌ Some checks failed. Please review the output above." -ForegroundColor Red
}

Write-Host "`nNext Steps:" -ForegroundColor Cyan
Write-Host "1. Start Docker Desktop and enable Kubernetes" -ForegroundColor White
Write-Host "2. Create GitHub repository for SecureShop API" -ForegroundColor White
Write-Host "3. Install VS Code extensions (see setup-guide.md)" -ForegroundColor White
Write-Host "4. Setup Git hooks with pre-commit" -ForegroundColor White
Write-Host "5. Proceed to Task 1.2: Create Sample .NET API" -ForegroundColor White
