# Tech Context: Technologies & Development Setup

## 🛠️ Technology Stack

### Core Languages & Frameworks
| Technology | Version | Purpose |
|------------|---------|---------|
| .NET | 8.0 | API development |
| C# | 12 | Primary language |
| PowerShell | 7.x | Scripting (Windows) |
| Bash | 5.x | Scripting (Linux/CI) |
| YAML | - | Configuration |

### Cloud Platform
| Service | Purpose |
|---------|---------|
| Azure Subscription | Cloud infrastructure |
| Azure DevOps | (Optional) Pipelines, Boards |
| Azure Container Registry | Container images |
| Azure Key Vault | Secrets management |
| Azure Kubernetes Service | Container orchestration |
| Azure Defender for Cloud | Security monitoring |
| Azure Policy | Governance |

### Container & Kubernetes
| Tool | Version | Purpose |
|------|---------|---------|
| Docker Desktop | Latest | Local containers |
| kubectl | Latest | K8s CLI |
| helm | 3.x | K8s package manager |
| Kyverno | Latest | Policy engine |

### Security Tools
| Category | Tool | Purpose |
|----------|------|---------|
| SAST | CodeQL | Static code analysis |
| SAST | SonarCloud | Code quality + security |
| SCA | Snyk | Dependency scanning |
| SCA | Dependabot | Auto dependency updates |
| Container | Trivy | Image vulnerability scanning |
| Secrets | TruffleHog | Secret detection in git |
| Secrets | detect-secrets | Pre-commit secret scanning |
| Signing | Cosign | Container image signing |
| SBOM | Syft | Software bill of materials |

### CI/CD
| Tool | Purpose |
|------|---------|
| GitHub Actions | Primary CI/CD |
| GitHub Advanced Security | CodeQL, secret scanning |

---

## 💻 Development Environment

### Operating System
- **Primary**: Windows 10/11
- **Shell**: PowerShell 7
- **WSL2**: Ubuntu (for Linux tools)

### IDE & Extensions
**VS Code Extensions**:
- Azure Tools
- Docker
- Kubernetes
- YAML
- C# Dev Kit
- GitLens
- SonarLint
- Trivy

### Local Tools Required
```powershell
# Check versions
az --version          # Azure CLI
kubectl version       # Kubernetes CLI
helm version          # Helm
docker --version      # Docker
dotnet --version      # .NET SDK
git --version         # Git
```

---

## 📋 Setup Instructions

### 1. Azure CLI
```powershell
# Install via MSI or winget
winget install Microsoft.AzureCLI

# Login
az login

# Set subscription
az account set --subscription "Your-Subscription-Name"
```

### 2. kubectl
```powershell
# Install via Azure CLI
az aks install-cli

# Or via chocolatey
choco install kubernetes-cli
```

### 3. Helm
```powershell
# Install via chocolatey
choco install kubernetes-helm

# Or via scoop
scoop install helm
```

### 4. Docker Desktop
- Download from docker.com
- Enable Kubernetes in settings
- Configure WSL2 backend

### 5. .NET SDK
```powershell
# Install via winget
winget install Microsoft.DotNet.SDK.8
```

### 6. Security Tools
```powershell
# Trivy (via scoop)
scoop install trivy

# Pre-commit
pip install pre-commit detect-secrets

# TruffleHog
docker pull trufflesecurity/trufflehog:latest
```

---

## ⚙️ Configuration Files

### Git Hooks (.pre-commit-config.yaml)
```yaml
repos:
  - repo: https://github.com/Yelp/detect-secrets
    rev: v1.4.0
    hooks:
      - id: detect-secrets
        args: ['--baseline', '.secrets.baseline']
```

### Dependabot (.github/dependabot.yml)
```yaml
version: 2
updates:
  - package-ecosystem: "nuget"
    directory: "/"
    schedule:
      interval: "weekly"
    open-pull-requests-limit: 10
```

### Docker Multi-stage Build Pattern
```dockerfile
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src
COPY . .
RUN dotnet publish -c Release -o /app

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
RUN adduser -D appuser
USER appuser
COPY --from=build /app .
ENTRYPOINT ["dotnet", "App.dll"]
```

---

## 🔒 Technical Constraints

### Azure Limitations
- Free tier: Limited resources
- ACR: Basic tier (limited features)
- AKS: Dev/Test cluster only

### Security Requirements
- No secrets in code
- All images signed
- Minimum 70% code coverage
- Zero critical/high vulnerabilities

### Performance Targets
- Pipeline duration: < 10 minutes
- Image size: < 150MB
- Scan time: < 2 minutes

---

## 📦 Dependencies

### NuGet Packages (Expected)
```xml
<PackageReference Include="Serilog" Version="x.x.x" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="x.x.x" />
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="x.x.x" />
```

### GitHub Actions
```yaml
uses: actions/checkout@v4
uses: actions/setup-dotnet@v4
uses: docker/build-push-action@v5
uses: azure/login@v1
uses: github/codeql-action/init@v2
uses: aquasecurity/trivy-action@master
```

---

## 🔗 Integration Points

### External Services
- **GitHub**: Source control, CI/CD, Security
- **SonarCloud**: Code quality analysis
- **Snyk**: Vulnerability database
- **Sigstore**: Keyless signing (optional)

### Azure Integration
- Service Principal for CI/CD
- Managed Identity for AKS workloads
- Key Vault for secrets
- ACR for images

---

**Created**: December 26, 2024  
**Last Updated**: December 26, 2024




