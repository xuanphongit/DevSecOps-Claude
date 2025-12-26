# DevSecOps Azure Learning Plan - Detailed Tasks & Context

## 📋 Overview
**Target Role**: Senior DevSecOps / Azure Security Engineer  
**Timeline**: 4 months (16 weeks)  
**Time Commitment**: 10-15 hours/week  
**Starting Project**: SecureShop API Platform

---

## 🎯 PROJECT 1: SecureShop API Platform (Months 1-4)

### **SPRINT 1: CI/CD Security Foundation (Weeks 1-4)**

---

#### **WEEK 1: Environment Setup & Basic Pipeline**

##### **Task 1.1: Setup Development Environment**
**Context**: Chuẩn bị môi trường làm việc với các tools cần thiết cho DevSecOps workflow.

**Sub-tasks**:
- [ ] Cài đặt Azure CLI, kubectl, helm, Docker Desktop
- [ ] Setup Azure DevOps organization (hoặc GitHub repo với Actions)
- [ ] Tạo Azure subscription (Free tier/Credits)
- [ ] Setup IDE extensions: YAML, Kubernetes, Azure Tools
- [ ] Cấu hình Git hooks cho pre-commit checks

**Acceptance Criteria**:
- ✓ Azure CLI login thành công
- ✓ Kubectl connect được tới local cluster
- ✓ Docker build/run image thành công
- ✓ Push code lên Azure Repos/GitHub

**Estimated Time**: 3-4 hours  
**Resources**: 
- [Azure CLI Docs](https://learn.microsoft.com/cli/azure/)
- [kubectl cheat sheet](https://kubernetes.io/docs/reference/kubectl/cheatsheet/)

---

##### **Task 1.2: Create Sample .NET API Application**
**Context**: Build một REST API đơn giản làm base để áp dụng security practices. API này sẽ có các endpoints cơ bản để test authentication, authorization, và data validation.

**Sub-tasks**:
- [ ] Tạo .NET 8 Web API project với template
- [ ] Implement 3-4 endpoints cơ bản:
  - `GET /api/products` - List products
  - `POST /api/products` - Create product (requires auth)
  - `GET /api/health` - Health check
  - `GET /api/metrics` - Basic metrics
- [ ] Add Swagger/OpenAPI documentation
- [ ] Configure logging với Serilog
- [ ] Write unit tests (min 70% coverage)
- [ ] Dockerize application (multi-stage build)

**Acceptance Criteria**:
- ✓ API chạy được trên local (http://localhost:5000)
- ✓ Swagger UI accessible
- ✓ Unit tests pass
- ✓ Docker image build thành công (<200MB)
- ✓ Container chạy được và health check OK

**Estimated Time**: 6-8 hours  
**Dependencies**: Task 1.1

**Code Structure**:
```
SecureShop.API/
├── Controllers/
├── Models/
├── Services/
├── Dockerfile
├── .dockerignore
└── SecureShop.Tests/
```

**Resources**:
- [.NET 8 Web API Tutorial](https://learn.microsoft.com/aspnet/core/tutorials/first-web-api)
- [Docker Best Practices](https://docs.docker.com/develop/dev-best-practices/)

---

##### **Task 1.3: Create Basic CI Pipeline (No Security Yet)**
**Context**: Tạo pipeline đơn giản để build và test application. Đây là baseline trước khi thêm security scanning.

**Sub-tasks**:
- [ ] Tạo `azure-pipelines.yml` hoặc `.github/workflows/ci.yml`
- [ ] Configure pipeline stages:
  ```yaml
  Stages:
  1. Restore dependencies
  2. Build application
  3. Run unit tests
  4. Build Docker image
  5. Publish artifacts
  ```
- [ ] Setup Azure Container Registry (ACR)
- [ ] Push image to ACR
- [ ] Test pipeline trigger on commit

**Acceptance Criteria**:
- ✓ Pipeline runs automatically on push
- ✓ All tests pass trong pipeline
- ✓ Docker image pushed to ACR
- ✓ Pipeline duration < 5 minutes

**Estimated Time**: 4-5 hours  
**Dependencies**: Task 1.2

**Pipeline Template**:
```yaml
trigger:
  - main

pool:
  vmImage: 'ubuntu-latest'

stages:
  - stage: Build
    jobs:
      - job: BuildAndTest
        steps:
          - task: UseDotNet@2
          - task: DotNetCoreCLI@2
            displayName: 'Restore'
          - task: DotNetCoreCLI@2
            displayName: 'Build'
          - task: DotNetCoreCLI@2
            displayName: 'Test'
          - task: Docker@2
            displayName: 'Build Image'
```

---

#### **WEEK 2: Security Scanning Integration**

##### **Task 2.1: Implement SAST (Static Application Security Testing)**
**Context**: Tích hợp công cụ quét mã nguồn để tìm các lỗ hổng bảo mật trong code như SQL injection, XSS, hardcoded secrets, insecure deserialization.

**Sub-tasks**:
- [ ] Setup GitHub Advanced Security (hoặc SonarCloud)
- [ ] Configure CodeQL analysis
  - Enable for C#
  - Custom queries cho .NET
- [ ] Setup SonarQube/SonarCloud
  - Configure quality gate
  - Set coverage threshold (min 70%)
- [ ] Add SAST stage to pipeline
- [ ] Fix 5 sample vulnerabilities (intentionally add first):
  - SQL injection vulnerability
  - Hardcoded API key
  - Weak password validation
  - Missing input validation
  - Insecure random generation

**Acceptance Criteria**:
- ✓ CodeQL scan runs on every PR
- ✓ SonarCloud shows security hotspots
- ✓ Pipeline fails if critical issues found
- ✓ Quality gate: A rating or above
- ✓ Zero critical/high vulnerabilities

**Estimated Time**: 6-8 hours  
**Dependencies**: Task 1.3

**CodeQL Configuration**:
```yaml
# .github/workflows/codeql.yml
- name: Initialize CodeQL
  uses: github/codeql-action/init@v2
  with:
    languages: csharp
    queries: security-extended

- name: Perform CodeQL Analysis
  uses: github/codeql-action/analyze@v2
```

**Resources**:
- [CodeQL Documentation](https://codeql.github.com/docs/)
- [OWASP Top 10](https://owasp.org/www-project-top-ten/)

---

##### **Task 2.2: Implement SCA (Software Composition Analysis)**
**Context**: Quét dependencies để phát hiện các thư viện có lỗ hổng bảo mật đã được công bố (CVE). Đây là một trong những attack vector phổ biến nhất.

**Sub-tasks**:
- [ ] Enable GitHub Dependabot
  - Configure `dependabot.yml`
  - Auto security updates
- [ ] Add Snyk scan to pipeline
  - Install Snyk CLI
  - Configure severity threshold
  - Generate vulnerability report
- [ ] Add Trivy filesystem scan
  - Scan dependencies
  - Scan IaC files
- [ ] Create dependency review process
- [ ] Document remediation workflow

**Acceptance Criteria**:
- ✓ Dependabot PRs được tạo tự động
- ✓ Snyk scan fails nếu có high/critical CVE
- ✓ Trivy report shows 0 critical issues
- ✓ Pipeline stage cho dependency check
- ✓ SBOM được generate (CycloneDX format)

**Estimated Time**: 5-6 hours  
**Dependencies**: Task 2.1

**Dependabot Config**:
```yaml
# .github/dependabot.yml
version: 2
updates:
  - package-ecosystem: "nuget"
    directory: "/"
    schedule:
      interval: "weekly"
    open-pull-requests-limit: 10
```

**Snyk Pipeline**:
```yaml
- task: SnykSecurityScan@1
  inputs:
    serviceConnectionEndpoint: 'Snyk'
    testType: 'app'
    severityThreshold: 'high'
    failOnIssues: true
```

---

##### **Task 2.3: Implement Secret Scanning**
**Context**: Ngăn chặn việc commit secrets (API keys, passwords, tokens) vào source code. Đây là lỗi bảo mật cơ bản nhưng rất phổ biến.

**Sub-tasks**:
- [ ] Enable GitHub Secret Scanning
- [ ] Setup TruffleHog in pipeline
- [ ] Add pre-commit hook với detect-secrets
- [ ] Configure custom regex patterns:
  - Azure connection strings
  - Database passwords
  - JWT tokens
  - Private keys
- [ ] Create secret rotation workflow
- [ ] Test với intentional secrets (revoked)

**Acceptance Criteria**:
- ✓ Pipeline fails khi detect secrets
- ✓ Pre-commit hook blocks secrets locally
- ✓ Scan cả Git history
- ✓ False positive < 5%
- ✓ Alerts sent to security team

**Estimated Time**: 4-5 hours  
**Dependencies**: Task 2.2

**TruffleHog Config**:
```yaml
- script: |
    docker run --rm -v "$(Build.SourcesDirectory):/repo" \
      trufflesecurity/trufflehog:latest \
      filesystem /repo --json --fail
  displayName: 'Scan for Secrets'
```

**Pre-commit Hook**:
```yaml
# .pre-commit-config.yaml
repos:
  - repo: https://github.com/Yelp/detect-secrets
    rev: v1.4.0
    hooks:
      - id: detect-secrets
        args: ['--baseline', '.secrets.baseline']
```

---

#### **WEEK 3: Container Security**

##### **Task 3.1: Harden Dockerfile**
**Context**: Áp dụng Docker security best practices để giảm attack surface và tránh các lỗ hổng phổ biến trong container images.

**Sub-tasks**:
- [ ] Convert sang multi-stage build
- [ ] Use non-root user
- [ ] Minimize image layers
- [ ] Remove unnecessary packages
- [ ] Use specific base image tags (not :latest)
- [ ] Implement health check
- [ ] Set resource limits
- [ ] Add security labels

**Current vs Hardened Dockerfile**:

**Before**:
```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app
COPY . .
RUN dotnet publish -c Release
EXPOSE 80
CMD ["dotnet", "SecureShop.API.dll"]
```

**After**:
```dockerfile
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0.1-alpine AS build
WORKDIR /src
COPY ["SecureShop.API/SecureShop.API.csproj", "SecureShop.API/"]
RUN dotnet restore "SecureShop.API/SecureShop.API.csproj"
COPY . .
WORKDIR "/src/SecureShop.API"
RUN dotnet build -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish \
    --no-restore \
    -p:PublishTrimmed=true

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0.1-alpine AS final
WORKDIR /app

# Create non-root user
RUN addgroup -g 1001 appgroup && \
    adduser -u 1001 -G appgroup -s /bin/sh -D appuser

# Copy published app
COPY --from=publish /app/publish .

# Set ownership
RUN chown -R appuser:appgroup /app

# Switch to non-root user
USER appuser

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=10s \
  CMD wget --no-verbose --tries=1 --spider http://localhost:8080/health || exit 1

EXPOSE 8080
ENTRYPOINT ["dotnet", "SecureShop.API.dll"]
```

**Acceptance Criteria**:
- ✓ Image size < 150MB (giảm từ ~200MB)
- ✓ Chạy với non-root user
- ✓ No critical vulnerabilities (Trivy scan)
- ✓ Health check working
- ✓ Hadolint score > 9/10

**Estimated Time**: 4-5 hours  
**Dependencies**: Task 2.3

---

##### **Task 3.2: Image Scanning with Trivy**
**Context**: Tự động quét container images để tìm CVE trong OS packages và application dependencies trước khi deploy.

**Sub-tasks**:
- [ ] Install Trivy in pipeline
- [ ] Configure scan policies:
  - Critical: FAIL
  - High: FAIL
  - Medium: WARN
  - Low: PASS
- [ ] Scan for:
  - OS vulnerabilities
  - Application dependencies
  - IaC misconfigurations
  - Secrets in layers
- [ ] Generate HTML report
- [ ] Publish scan results as artifact
- [ ] Setup automated remediation PR

**Acceptance Criteria**:
- ✓ Trivy scan runs after image build
- ✓ Pipeline fails on critical/high
- ✓ Scan results uploaded to Azure/GitHub
- ✓ Scan time < 2 minutes
- ✓ Zero false positives

**Estimated Time**: 3-4 hours  
**Dependencies**: Task 3.1

**Pipeline Integration**:
```yaml
- task: Bash@3
  displayName: 'Trivy Scan'
  inputs:
    targetType: 'inline'
    script: |
      # Install Trivy
      wget -qO - https://aquasecurity.github.io/trivy-repo/deb/public.key | sudo apt-key add -
      echo "deb https://aquasecurity.github.io/trivy-repo/deb $(lsb_release -sc) main" | sudo tee -a /etc/apt/sources.list.d/trivy.list
      sudo apt-get update
      sudo apt-get install trivy
      
      # Scan image
      trivy image \
        --severity CRITICAL,HIGH \
        --exit-code 1 \
        --format json \
        --output trivy-report.json \
        $(containerRegistry)/$(imageName):$(Build.BuildId)
      
      # Generate HTML report
      trivy image \
        --severity CRITICAL,HIGH,MEDIUM \
        --format template \
        --template "@contrib/html.tpl" \
        --output trivy-report.html \
        $(containerRegistry)/$(imageName):$(Build.BuildId)

- task: PublishPipelineArtifact@1
  displayName: 'Publish Trivy Report'
  inputs:
    targetPath: 'trivy-report.html'
    artifact: 'TrivyScanReport'
```

---

##### **Task 3.3: Image Signing with Cosign**
**Context**: Đảm bảo image integrity và authenticity bằng cách ký images. Đây là phần quan trọng của supply chain security.

**Sub-tasks**:
- [ ] Setup Sigstore Cosign
- [ ] Generate signing key pair
- [ ] Store private key in Azure Key Vault
- [ ] Sign images in pipeline
- [ ] Configure ACR to require signed images
- [ ] Create image verification policy
- [ ] Test với unsigned image (should fail)

**Acceptance Criteria**:
- ✓ Mọi image được sign sau build
- ✓ Signature stored in OCI registry
- ✓ Verification working in AKS
- ✓ Unsigned images rejected
- ✓ Key rotation documented

**Estimated Time**: 5-6 hours  
**Dependencies**: Task 3.2

**Signing Pipeline**:
```yaml
- task: AzureCLI@2
  displayName: 'Sign Container Image'
  inputs:
    azureSubscription: '$(azureServiceConnection)'
    scriptType: 'bash'
    scriptLocation: 'inlineScript'
    inlineScript: |
      # Login to ACR
      az acr login --name $(containerRegistry)
      
      # Get signing key from Key Vault
      az keyvault secret download \
        --vault-name $(keyVaultName) \
        --name cosign-private-key \
        --file cosign.key
      
      # Sign image
      export COSIGN_PASSWORD=$(az keyvault secret show \
        --vault-name $(keyVaultName) \
        --name cosign-password \
        --query value -o tsv)
      
      cosign sign \
        --key cosign.key \
        $(containerRegistry)/$(imageName):$(Build.BuildId)
      
      # Cleanup
      rm cosign.key
```

---

#### **WEEK 4: Security Gate & SBOM**

##### **Task 4.1: Implement Security Quality Gate**
**Context**: Tạo automated decision point trong pipeline để quyết định có deploy hay không dựa trên security findings.

**Sub-tasks**:
- [ ] Define security thresholds:
  ```
  Critical CVE: 0 allowed
  High CVE: 0 allowed
  Medium CVE: < 5 allowed
  SAST: No critical/high
  Secret scan: 0 detected
  Code coverage: >= 70%
  ```
- [ ] Create custom pipeline task
- [ ] Aggregate scan results
- [ ] Generate security scorecard
- [ ] Implement override mechanism (với approval)
- [ ] Send notifications on failures

**Acceptance Criteria**:
- ✓ Gate blocks deployment nếu fail
- ✓ Clear failure reason displayed
- ✓ Scorecard published to dashboard
- ✓ Override requires security team approval
- ✓ Metrics tracked over time

**Estimated Time**: 6-7 hours  
**Dependencies**: Tasks 2.1, 2.2, 2.3, 3.2

**Gate Logic**:
```yaml
- stage: SecurityGate
  dependsOn: SecurityScanning
  jobs:
    - job: EvaluateSecurityPosture
      steps:
        - task: PowerShell@2
          displayName: 'Security Gate Evaluation'
          inputs:
            targetType: 'inline'
            script: |
              # Collect results from previous stages
              $trivyCritical = $(trivy.critical)
              $trivyHigh = $(trivy.high)
              $snykIssues = $(snyk.issues)
              $secretsFound = $(secrets.count)
              $sastCritical = $(sast.critical)
              
              # Evaluate thresholds
              $passed = $true
              $reasons = @()
              
              if ($trivyCritical -gt 0) {
                $passed = $false
                $reasons += "Trivy: $trivyCritical critical vulnerabilities"
              }
              
              if ($trivyHigh -gt 0) {
                $passed = $false
                $reasons += "Trivy: $trivyHigh high vulnerabilities"
              }
              
              if ($snykIssues -gt 0) {
                $passed = $false
                $reasons += "Snyk: $snykIssues critical dependencies"
              }
              
              if ($secretsFound -gt 0) {
                $passed = $false
                $reasons += "Secret scanning: $secretsFound secrets detected"
              }
              
              if ($sastCritical -gt 0) {
                $passed = $false
                $reasons += "SAST: $sastCritical critical issues"
              }
              
              # Output results
              if ($passed) {
                Write-Host "✅ Security Gate: PASSED"
                exit 0
              } else {
                Write-Host "❌ Security Gate: FAILED"
                Write-Host "Reasons:"
                $reasons | ForEach-Object { Write-Host "  - $_" }
                exit 1
              }
```

---

##### **Task 4.2: Generate SBOM (Software Bill of Materials)**
**Context**: Tạo danh sách đầy đủ các components trong application để tracking vulnerabilities và compliance.

**Sub-tasks**:
- [ ] Setup Syft for SBOM generation
- [ ] Generate CycloneDX format SBOM
- [ ] Generate SPDX format SBOM
- [ ] Include:
  - Application dependencies
  - OS packages
  - Container base image info
  - Build metadata
- [ ] Store SBOM in artifact repository
- [ ] Create SBOM viewer/dashboard
- [ ] Setup SBOM comparison (track changes)

**Acceptance Criteria**:
- ✓ SBOM generated for every build
- ✓ Both CycloneDX and SPDX formats
- ✓ SBOM uploaded to storage account
- ✓ SBOM contains 100% of dependencies
- ✓ Viewable in security dashboard

**Estimated Time**: 4-5 hours  
**Dependencies**: Task 3.2

**SBOM Generation**:
```yaml
- task: Bash@3
  displayName: 'Generate SBOM'
  inputs:
    targetType: 'inline'
    script: |
      # Install Syft
      curl -sSfL https://raw.githubusercontent.com/anchore/syft/main/install.sh | sh -s -- -b /usr/local/bin
      
      # Generate CycloneDX SBOM
      syft $(containerRegistry)/$(imageName):$(Build.BuildId) \
        -o cyclonedx-json \
        > sbom-cyclonedx.json
      
      # Generate SPDX SBOM
      syft $(containerRegistry)/$(imageName):$(Build.BuildId) \
        -o spdx-json \
        > sbom-spdx.json
      
      # Add build metadata
      jq '. + {
        "metadata": {
          "buildId": "$(Build.BuildId)",
          "buildDate": "'$(date -u +%Y-%m-%dT%H:%M:%SZ)'",
          "commitSha": "$(Build.SourceVersion)",
          "repository": "$(Build.Repository.Name)"
        }
      }' sbom-cyclonedx.json > sbom-final.json

- task: AzureCLI@2
  displayName: 'Upload SBOM to Storage'
  inputs:
    azureSubscription: '$(azureServiceConnection)'
    scriptType: 'bash'
    scriptLocation: 'inlineScript'
    inlineScript: |
      az storage blob upload \
        --account-name $(storageAccountName) \
        --container-name sbom \
        --name $(imageName)-$(Build.BuildId).json \
        --file sbom-final.json \
        --auth-mode login
```

---

##### **Task 4.3: Security Dashboard Setup**
**Context**: Tạo centralized view để track security metrics, trends, và compliance status.

**Sub-tasks**:
- [ ] Setup Azure Workbook hoặc Grafana
- [ ] Create dashboards:
  - Pipeline security metrics
  - Vulnerability trends
  - SBOM inventory
  - Compliance status
- [ ] Configure data sources:
  - Azure DevOps analytics
  - Defender for Cloud
  - Log Analytics
- [ ] Add visualizations:
  - Security scorecard
  - Vulnerability heatmap
  - Scan history
  - Mean time to remediate
- [ ] Setup alerts for thresholds

**Acceptance Criteria**:
- ✓ Dashboard accessible to team
- ✓ Real-time data updates
- ✓ Historical trend analysis
- ✓ Exportable reports
- ✓ Mobile-friendly view

**Estimated Time**: 5-6 hours  
**Dependencies**: Task 4.1, 4.2

**Dashboard Metrics**:
```json
{
  "metrics": [
    "total_builds",
    "security_gate_pass_rate",
    "critical_vulnerabilities",
    "high_vulnerabilities",
    "mean_time_to_fix",
    "sbom_generation_success_rate",
    "secrets_detected",
    "compliance_score"
  ],
  "charts": [
    "vulnerability_trend",
    "security_score_history",
    "scan_duration",
    "dependency_risk_distribution"
  ]
}
```

---

### **📊 SPRINT 1 DELIVERABLES CHECKLIST**

**By end of Week 4, you should have:**

- [ ] ✅ Fully automated CI/CD pipeline
- [ ] ✅ 5+ security scanning tools integrated
- [ ] ✅ Security gate blocks vulnerable code
- [ ] ✅ Signed container images
- [ ] ✅ SBOM generated automatically
- [ ] ✅ Security dashboard operational
- [ ] ✅ Documentation completed
- [ ] ✅ Demo video recorded

**Metrics to Track**:
```
Pipeline Security Score: __/100
├── SAST Coverage: __/20
├── SCA Integration: __/20
├── Secret Scanning: __/15
├── Container Security: __/25
├── SBOM Generation: __/10
└── Gate Enforcement: __/10

Current Stats:
├── Total Builds: __
├── Gate Pass Rate: __%
├── Critical Vulnerabilities Fixed: __
└── Average Scan Time: __ minutes
```

---

## 📝 How to Use This Plan

### **Daily Workflow**:
1. **Start of session**: Check off completed sub-tasks
2. **During work**: Reference context & acceptance criteria
3. **End of session**: Update progress, note blockers
4. **Weekly review**: Assess if on track, adjust timeline

### **When Stuck**:
1. Review the context section
2. Check resources links
3. Search for specific error messages
4. Ask for help with specific task reference (e.g., "Task 2.1")

### **Progress Tracking**:
```bash
# Use this format to track
[Week 1] ████████░░ 80% (3.5/4 tasks)
[Week 2] ███░░░░░░░ 30% (1/3 tasks)
[Week 3] ░░░░░░░░░░  0% (0/3 tasks)
[Week 4] ░░░░░░░░░░  0% (0/3 tasks)
```

---

## 🎯 Next Steps

After completing Sprint 1, you'll move to:
**SPRINT 2: AKS Security (Weeks 5-8)**

Preview of Sprint 2 tasks:
- Setup AKS cluster with security baseline
- Implement RBAC and Pod Security Standards
- Configure network policies
- Setup admission controllers (Kyverno/OPA)
- Integrate Defender for Kubernetes

---

## 📚 Additional Resources

### **Communities**:
- [r/devops](https://reddit.com/r/devops)
- [DevSecOps Community on Discord](https://discord.gg/devsecops)
- [CNCF Slack - #security](https://slack.cncf.io)

### **Newsletters**:
- tl;dr sec (security newsletter)
- DevOps Weekly
- Kubernetes Security Newsletter

### **Practice Platforms**:
- [TryHackMe - DevSecOps Path](https://tryhackme.com)
- [HackTheBox - Secure Coding](https://hackthebox.com)
- [Microsoft Learn Sandbox](https://learn.microsoft.com/training/)

---

**Created**: December 2024  
**Last Updated**: [Date]  
**Version**: 1.0  
**Status**: 🟡 In Progress - Sprint 1
