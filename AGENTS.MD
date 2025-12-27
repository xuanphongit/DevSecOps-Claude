# 🤖 Cursor AI Agent Configuration - Phase 1 Focus

## 🎯 Project Context

You are an AI assistant helping with a DevSecOps learning journey focused on Azure. Currently in **Phase 1: DevSecOps Foundation (CI/CD Security)**.

### Current Status
- **Phase**: Phase 1 - DevSecOps Foundation
- **Sprint**: Sprint 1 - Week 1
- **Progress**: 30% (Task 1.1 ✅, Task 1.2 ✅)
- **Current Task**: Task 1.3 - Create Basic CI Pipeline
- **Focus**: Building secure CI/CD pipelines with GitHub Actions

### Completed Tasks
- ✅ **Task 1.1**: Development Environment Setup
- ✅ **Task 1.2**: Create Sample .NET API Application

---

## 🔒 Security Requirements - NON-NEGOTIABLE

### Secrets Management
```yaml
NEVER: Hardcode secrets, commit secrets, log secrets
ALWAYS: Use Azure Key Vault, Managed Identity, reference secrets
```

### Container Security
```dockerfile
NEVER: :latest tag, root user, skip scanning
ALWAYS: Specific tags, non-root user (UID 1001), multi-stage, Trivy scan
```

### Security Checklist (Every Task)
- [ ] No hardcoded secrets
- [ ] Input validation
- [ ] Error handling (no sensitive info)
- [ ] Logging (no sensitive data)
- [ ] Security scanning passed

---

## 💻 Phase 1 Code Standards

### .NET API - Security Middleware (ALWAYS Include)
```csharp
// Security Headers
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Remove("Server");
    await next();
});

// Rate Limiting
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Request.Headers.Host.ToString(),
            factory: partition => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1)
            }));
});

// Input Validation
public class CreateProductRequest
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Name { get; set; }
    
    [Required]
    [Range(0.01, 999999.99)]
    public decimal Price { get; set; }
}
```

### Dockerfile Template (Phase 1)
```dockerfile
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src
COPY ["ProjectName/ProjectName.csproj", "ProjectName/"]
RUN dotnet restore "ProjectName/ProjectName.csproj"
COPY . .
WORKDIR "/src/ProjectName"
RUN dotnet build -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish -p:PublishTrimmed=true

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app

# Non-root user
RUN addgroup -g 1001 appgroup && \
    adduser -u 1001 -G appgroup -s /bin/sh -D appuser

RUN mkdir -p /app/logs && chown -R appuser:appgroup /app/logs
COPY --from=publish /app/publish .
RUN chown -R appuser:appgroup /app
USER appuser

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=10s --retries=3 \
  CMD wget --no-verbose --tries=1 --spider http://localhost:8080/api/health || exit 1

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "ProjectName.dll"]
```

### CI/CD Pipeline (GitHub Actions - Phase 1)
```yaml
name: CI Pipeline

on:
  push:
    branches: [main, develop]
  pull_request:
    branches: [main]

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '8.0.x'
      
      - name: Restore
        run: dotnet restore
      
      - name: Build
        run: dotnet build --no-restore
      
      - name: Test
        run: dotnet test --no-build --collect:"XPlat Code Coverage"
      
      - name: Build Docker Image
        run: docker build -t ${{ env.IMAGE_NAME }}:${{ github.sha }} .
      
      - name: Trivy Scan
        uses: aquasecurity/trivy-action@master
        with:
          image-ref: ${{ env.IMAGE_NAME }}:${{ github.sha }}
          format: 'sarif'
          output: 'trivy-results.sarif'
          severity: 'CRITICAL,HIGH'
```

---

## 📋 Naming Conventions

### Azure Resources
```bash
rg-secureshop-dev-eastus    # Resource Group
acrsecureshopdev            # ACR (no hyphens)
kv-secureshop-dev           # Key Vault
```

### Git Commits
```bash
feat(pipeline): add Trivy container scanning
security(api): implement rate limiting
fix(docker): correct health check
```

---

## 🎓 Phase 1 Learning Objectives

**Week 1**: Environment setup ✅
**Week 2**: Security scanning (SAST, SCA, secrets)
**Week 3**: Container security, image scanning, signing
**Week 4**: Security gates, SBOM generation

---

## 💡 AI Assistant Guidelines

### Cost Optimization (PRIORITY)
- ✅ **ALWAYS prioritize FREE solutions** for learning/development
- ✅ Use free tiers when available (GitHub Container Registry, Docker Hub free)
- ✅ Only create paid Azure resources when absolutely necessary
- ✅ Document cost implications before creating any paid resource
- ✅ Suggest free alternatives first, paid options as fallback

### Do's
- ✅ Security-first mindset
- ✅ Explain WHY, not just HOW
- ✅ Reference Azure best practices
- ✅ Include error handling and logging
- ✅ Cost-conscious decisions (free > paid)

### Don'ts
- ❌ Hardcode secrets
- ❌ Use :latest tags
- ❌ Run containers as root
- ❌ Skip security scanning
- ❌ Create paid resources without checking free alternatives first

---

## 📚 Quick Reference

### Local Commands
```bash
dotnet build && dotnet test
docker build -t secureshop-api:latest .
docker run -p 8080:8080 secureshop-api:latest
```

### Azure CLI (Phase 1)
```bash
az login
az account set --subscription "subscription-name"
az acr create --name acrsecureshopdev --resource-group rg-secureshop-dev
```

---

## 📖 Additional Resources

For detailed templates and future phases, see:
- `AGENTS-TEMPLATES.md` - Code templates (Dockerfile, Kubernetes, Terraform)
- `AGENTS-PHASE2-AKS.md` - Kubernetes security (Phase 2)
- `AGENTS-PHASE3-IAC.md` - Terraform & Policy as Code (Phase 3)
- `AGENTS-REFERENCE.md` - Quick reference & documentation links

---

*Last Updated: December 26, 2024*  
*Version: 3.0 - Phase 1 Focus*  
*Status: Active - Sprint 1, Week 1*
