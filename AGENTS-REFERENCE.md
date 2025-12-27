# 📚 Quick Reference & Documentation

Quick reference for common commands, links, and patterns.

---

## 🛠️ Technical Stack

### Core Azure Services (Phase 1)
- **DevOps**: GitHub Actions, Azure Pipelines
- **Container**: Azure Container Registry (ACR)
- **Security**: Key Vault, Defender for Cloud
- **Monitoring**: Log Analytics, Application Insights

### Security Tools (Phase 1)
- **SAST**: CodeQL, SonarQube
- **SCA**: Snyk, Trivy, Dependabot
- **Container**: Trivy, Cosign, Syft
- **Secrets**: detect-secrets, TruffleHog

---

## 📐 Naming Conventions

### Azure Resources
```bash
rg-secureshop-dev-eastus    # Resource Group
acrsecureshopdev            # ACR (no hyphens)
kv-secureshop-dev           # Key Vault
st-secureshop-dev           # Storage Account
la-secureshop-dev           # Log Analytics
```

### Kubernetes Resources
```yaml
secureshop-api-deployment
secureshop-api-service
secureshop-api-networkpolicy
```

### Git Branches & Commits
```bash
# Branches
feature/task-1.2-implement-sast
fix/pipeline-timeout-issue
security/add-network-policies

# Commits: <type>(<scope>): <subject>
feat(pipeline): add Trivy container scanning
security(api): implement rate limiting middleware
fix(docker): correct health check
```

---

## 💻 Common Commands

### Local Development
```bash
# Build & Test
dotnet build
dotnet test

# Security Scans
docker run --rm -v $(pwd):/src aquasec/trivy fs /src
detect-secrets scan

# Container
docker build -t secureshop-api:latest .
docker run --rm -p 8080:8080 secureshop-api:latest
```

### Azure CLI
```bash
# Login and set subscription
az login
az account set --subscription "subscription-name"

# Create resource group
az group create --name rg-secureshop-prod --location eastus

# Create ACR
az acr create \
  --name acrsecureshopdev \
  --resource-group rg-secureshop-dev \
  --sku Basic

# Create Key Vault
az keyvault create \
  --name kv-secureshop-prod \
  --resource-group rg-secureshop-prod \
  --enable-rbac-authorization \
  --enable-purge-protection
```

### Kubernetes (Phase 2+)
```bash
kubectl apply -k kubernetes/base
kubectl get pods -w
kubectl get networkpolicies
```

### Terraform (Phase 3+)
```bash
terraform init
terraform plan
terraform apply
tfsec .
checkov -d .
```

---

## 📚 Reference Documentation

### Security Standards
- [Azure Security Baseline](https://learn.microsoft.com/security/benchmark/azure/)
- [OWASP Top 10](https://owasp.org/www-project-top-ten/)
- [CIS Docker Benchmark](https://www.cisecurity.org/benchmark/docker)
- [CIS Kubernetes Benchmark](https://www.cisecurity.org/benchmark/kubernetes)

### Azure Documentation
- [Azure Well-Architected Framework](https://learn.microsoft.com/azure/well-architected/)
- [Kubernetes Security Best Practices](https://kubernetes.io/docs/concepts/security/)
- [Azure Key Vault Documentation](https://learn.microsoft.com/azure/key-vault/)

### Tools Documentation
- [Trivy Documentation](https://aquasecurity.github.io/trivy/)
- [CodeQL Documentation](https://codeql.github.com/docs/)
- [Cosign Documentation](https://docs.sigstore.dev/cosign/overview/)

---

## 🎯 Success Metrics

- Security scanning coverage: Target 100%
- Vulnerability remediation time: < 48 hours for critical
- Deployment frequency: Daily for dev, weekly for prod
- Security gate pass rate: > 95%
- Policy compliance: 100% for mandatory policies

---

*Quick reference for all phases. See AGENTS.MD for Phase 1 specific rules.*
