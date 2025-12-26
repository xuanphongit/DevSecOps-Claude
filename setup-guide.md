# Development Environment Setup Guide

## ✅ Current Status

### Installed Tools
- ✅ Azure CLI 2.80.0 (update available: run `az upgrade`)
- ✅ kubectl v1.33.2
- ✅ helm v3.18.2
- ✅ Docker 29.0.1
- ✅ Azure subscription: Pay-As-You-Go (Active)

### Action Required
- ⚠️ Docker Desktop needs to be started
- ⚠️ Kubernetes cluster needs to be enabled in Docker Desktop

---

## 🔧 Setup Steps

### 1. Start Docker Desktop & Enable Kubernetes

1. **Start Docker Desktop**
   - Open Docker Desktop from Start Menu
   - Wait for it to fully start (whale icon in system tray)

2. **Enable Kubernetes**
   - Open Docker Desktop Settings
   - Go to **Kubernetes** tab
   - Check **"Enable Kubernetes"**
   - Click **Apply & Restart**
   - Wait for Kubernetes to start (green indicator)

3. **Verify Kubernetes is Running**
   ```powershell
   kubectl cluster-info
   kubectl get nodes
   ```

### 2. Update Azure CLI (Optional but Recommended)

```powershell
az upgrade
```

### 3. Verify Azure Login

```powershell
# Check current subscription
az account show

# List all subscriptions
az account list --output table

# Set default subscription if needed
az account set --subscription "Pay-As-You-Go"
```

### 4. Test Docker

```powershell
# Verify Docker is running
docker ps

# Test Docker build
docker run hello-world

# Test Docker build (if you have a Dockerfile)
docker build -t test-image .
```

### 5. Test Kubernetes

```powershell
# Check cluster status
kubectl cluster-info

# List nodes
kubectl get nodes

# Create a test pod
kubectl run test-pod --image=nginx --restart=Never
kubectl get pods
kubectl delete pod test-pod
```

### 6. Test Helm

```powershell
# Verify Helm installation
helm version

# Add a test chart repository
helm repo add stable https://charts.helm.sh/stable
helm repo update
```

---

## 📁 GitHub Repository Setup

### Option 1: Create New Repository

1. Go to [GitHub](https://github.com/new)
2. Repository name: `secureshop-api` (or your preferred name)
3. Description: "SecureShop API Platform - DevSecOps Learning Project"
4. Visibility: Public (for portfolio) or Private
5. Initialize with README: ✅
6. Add .gitignore: .NET
7. Click **Create repository**

### Option 2: Use Existing Repository

If you already have a repository, clone it:

```powershell
git clone https://github.com/YOUR_USERNAME/secureshop-api.git
cd secureshop-api
```

---

## 🔌 VS Code Extensions Setup

### Required Extensions

Install these extensions in VS Code:

1. **Azure Tools** (ms-azuretools.vscode-azureresourcegroups)
   - Azure Account
   - Azure Resources
   - Azure CLI Tools

2. **Docker** (ms-azuretools.vscode-docker)
   - Docker extension pack

3. **Kubernetes** (ms-kubernetes-tools.vscode-kubernetes-tools)
   - Kubernetes extension pack

4. **YAML** (redhat.vscode-yaml)
   - YAML language support

5. **C# Dev Kit** (ms-dotnettools.csdevkit)
   - C# development tools

6. **GitLens** (eamodio.gitlens)
   - Enhanced Git capabilities

7. **SonarLint** (SonarSource.sonarlint-vscode)
   - Code quality analysis

8. **Trivy** (aquasecurity.trivy-vscode-extension)
   - Security scanning

### Quick Install (PowerShell)

```powershell
# Install via VS Code CLI
code --install-extension ms-azuretools.vscode-azureresourcegroups
code --install-extension ms-azuretools.vscode-docker
code --install-extension ms-kubernetes-tools.vscode-kubernetes-tools
code --install-extension redhat.vscode-yaml
code --install-extension ms-dotnettools.csdevkit
code --install-extension eamodio.gitlens
code --install-extension SonarSource.sonarlint-vscode
code --install-extension aquasecurity.trivy-vscode-extension
```

---

## 🔒 Git Hooks Setup (Pre-commit Secret Scanning)

### Step 1: Install Pre-commit

```powershell
# Install Python if not already installed
# Then install pre-commit
pip install pre-commit detect-secrets
```

### Step 2: Create Pre-commit Configuration

Create `.pre-commit-config.yaml` in your repository root:

```yaml
repos:
  - repo: https://github.com/Yelp/detect-secrets
    rev: v1.4.0
    hooks:
      - id: detect-secrets
        args: ['--baseline', '.secrets.baseline']
        exclude: package.lock.json

  - repo: https://github.com/pre-commit/pre-commit-hooks
    rev: v4.5.0
    hooks:
      - id: trailing-whitespace
      - id: end-of-file-fixer
      - id: check-yaml
      - id: check-added-large-files
      - id: check-json
```

### Step 3: Initialize Pre-commit

```powershell
# Navigate to your repository
cd path/to/secureshop-api

# Initialize pre-commit
pre-commit install

# Create baseline (first time only)
detect-secrets scan > .secrets.baseline
```

### Step 4: Test Pre-commit Hook

```powershell
# Test the hook
pre-commit run --all-files

# Try committing a file with a secret (should fail)
echo "password=secret123" > test.txt
git add test.txt
git commit -m "test"  # Should be blocked
```

---

## ✅ Verification Checklist

Run this verification script to check everything:

```powershell
# Save as verify-setup.ps1
Write-Host "=== Development Environment Verification ===" -ForegroundColor Cyan

# Azure CLI
Write-Host "`n[1] Checking Azure CLI..." -ForegroundColor Yellow
$azVersion = az --version 2>&1 | Select-String "azure-cli"
if ($azVersion) {
    Write-Host "✅ Azure CLI installed" -ForegroundColor Green
    $account = az account show 2>&1 | ConvertFrom-Json
    if ($account) {
        Write-Host "✅ Azure logged in: $($account.name)" -ForegroundColor Green
    } else {
        Write-Host "❌ Azure not logged in. Run: az login" -ForegroundColor Red
    }
} else {
    Write-Host "❌ Azure CLI not found" -ForegroundColor Red
}

# kubectl
Write-Host "`n[2] Checking kubectl..." -ForegroundColor Yellow
$kubectlVersion = kubectl version --client 2>&1
if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ kubectl installed" -ForegroundColor Green
    $cluster = kubectl cluster-info 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✅ Kubernetes cluster accessible" -ForegroundColor Green
    } else {
        Write-Host "⚠️  Kubernetes cluster not running. Start Docker Desktop and enable Kubernetes" -ForegroundColor Yellow
    }
} else {
    Write-Host "❌ kubectl not found" -ForegroundColor Red
}

# Helm
Write-Host "`n[3] Checking Helm..." -ForegroundColor Yellow
$helmVersion = helm version 2>&1
if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Helm installed" -ForegroundColor Green
} else {
    Write-Host "❌ Helm not found" -ForegroundColor Red
}

# Docker
Write-Host "`n[4] Checking Docker..." -ForegroundColor Yellow
$dockerVersion = docker --version 2>&1
if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Docker installed" -ForegroundColor Green
    $dockerPs = docker ps 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✅ Docker daemon running" -ForegroundColor Green
    } else {
        Write-Host "⚠️  Docker daemon not running. Start Docker Desktop" -ForegroundColor Yellow
    }
} else {
    Write-Host "❌ Docker not found" -ForegroundColor Red
}

# Git
Write-Host "`n[5] Checking Git..." -ForegroundColor Yellow
$gitVersion = git --version 2>&1
if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Git installed" -ForegroundColor Green
} else {
    Write-Host "❌ Git not found" -ForegroundColor Red
}

# Pre-commit
Write-Host "`n[6] Checking Pre-commit..." -ForegroundColor Yellow
$precommitVersion = pre-commit --version 2>&1
if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Pre-commit installed" -ForegroundColor Green
} else {
    Write-Host "⚠️  Pre-commit not installed. Run: pip install pre-commit detect-secrets" -ForegroundColor Yellow
}

Write-Host "`n=== Verification Complete ===" -ForegroundColor Cyan
```

---

## 🎯 Acceptance Criteria Status

| Criteria | Status | Notes |
|----------|--------|-------|
| Azure CLI login | ✅ | Logged in to Pay-As-You-Go |
| kubectl connect to cluster | ⚠️ | Need to start Docker Desktop Kubernetes |
| Docker build/run | ⚠️ | Need to start Docker Desktop |
| Push code to GitHub | ⏳ | Repository setup pending |

---

## 📝 Next Steps

1. **Start Docker Desktop** and enable Kubernetes
2. **Create GitHub repository** for SecureShop API
3. **Install VS Code extensions**
4. **Setup Git hooks** with pre-commit
5. **Run verification script** to confirm everything works
6. **Proceed to Task 1.2**: Create Sample .NET API Application

---

## 🆘 Troubleshooting

### Docker Desktop Won't Start
- Check Windows WSL2 is installed: `wsl --status`
- Update WSL2: `wsl --update`
- Restart computer

### Kubernetes Won't Enable
- Ensure Docker Desktop is fully started
- Check system resources (RAM/CPU)
- Try disabling and re-enabling Kubernetes

### Azure CLI Login Issues
- Clear cache: `az account clear`
- Login again: `az login`
- Use specific tenant: `az login --tenant YOUR_TENANT_ID`

### Pre-commit Not Working
- Ensure Python is in PATH
- Reinstall: `pip install --upgrade pre-commit detect-secrets`
- Reinitialize: `pre-commit install --install-hooks`

---

**Last Updated**: December 26, 2024  
**Status**: Setup in Progress
