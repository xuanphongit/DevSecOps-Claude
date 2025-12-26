# DevSecOps Learning Project - Structure & Organization

## 📁 Recommended Folder Structure

```
devsecops-azure-journey/
│
├── 📋 planning/
│   ├── devsecops-learning-plan.md          # Main detailed plan
│   ├── task-tracker.md                      # Daily checklist
│   ├── task-completion-template.md          # Template for docs
│   ├── sprint-retrospectives.md             # Sprint reviews
│   └── roadmap.md                           # Long-term roadmap
│
├── 📚 docs/
│   ├── completed-tasks/
│   │   ├── task-1.1-environment-setup.md
│   │   ├── task-1.2-dotnet-api.md
│   │   └── ...
│   ├── guides/
│   │   ├── azure-setup-guide.md
│   │   ├── troubleshooting.md
│   │   └── best-practices.md
│   ├── architecture/
│   │   ├── diagrams/
│   │   ├── security-architecture.md
│   │   └── infrastructure-design.md
│   └── research/
│       ├── tools-evaluation.md
│       ├── security-patterns.md
│       └── azure-services-comparison.md
│
├── 🚀 projects/
│   │
│   ├── project-1-secureshop-api/
│   │   ├── src/
│   │   │   ├── SecureShop.API/
│   │   │   │   ├── Controllers/
│   │   │   │   ├── Models/
│   │   │   │   ├── Services/
│   │   │   │   ├── Program.cs
│   │   │   │   └── Dockerfile
│   │   │   ├── SecureShop.Tests/
│   │   │   └── SecureShop.sln
│   │   │
│   │   ├── pipelines/
│   │   │   ├── azure-pipelines.yml
│   │   │   ├── ci-pipeline.yml
│   │   │   ├── security-scan.yml
│   │   │   └── deploy-pipeline.yml
│   │   │
│   │   ├── security/
│   │   │   ├── policies/
│   │   │   │   ├── pod-security-policy.yaml
│   │   │   │   └── network-policy.yaml
│   │   │   ├── scans/
│   │   │   │   ├── trivy-reports/
│   │   │   │   ├── sonar-reports/
│   │   │   │   └── snyk-reports/
│   │   │   └── sbom/
│   │   │       ├── sbom-cyclonedx.json
│   │   │       └── sbom-spdx.json
│   │   │
│   │   ├── infrastructure/
│   │   │   ├── terraform/
│   │   │   │   ├── main.tf
│   │   │   │   ├── variables.tf
│   │   │   │   └── outputs.tf
│   │   │   └── kubernetes/
│   │   │       ├── base/
│   │   │       └── overlays/
│   │   │
│   │   ├── docs/
│   │   │   ├── README.md
│   │   │   ├── SECURITY.md
│   │   │   ├── ARCHITECTURE.md
│   │   │   └── deployment-guide.md
│   │   │
│   │   └── .github/ (or .azure/)
│   │       ├── workflows/
│   │       ├── dependabot.yml
│   │       └── codeql-analysis.yml
│   │
│   ├── project-2-landing-zone/
│   │   ├── terraform/
│   │   │   ├── modules/
│   │   │   │   ├── management-groups/
│   │   │   │   ├── subscriptions/
│   │   │   │   ├── networking/
│   │   │   │   ├── security/
│   │   │   │   └── monitoring/
│   │   │   ├── environments/
│   │   │   │   ├── dev/
│   │   │   │   ├── staging/
│   │   │   │   └── prod/
│   │   │   └── policies/
│   │   │       ├── custom-policies/
│   │   │       └── initiatives/
│   │   └── docs/
│   │
│   └── project-3-zero-trust-api/
│       ├── [similar structure]
│       └── ...
│
├── 🎯 certifications/
│   ├── cks/
│   │   ├── study-notes.md
│   │   ├── practice-tests/
│   │   └── labs/
│   ├── az-500/
│   └── sc-100/
│
├── 📊 metrics/
│   ├── dashboards/
│   │   ├── azure-workbooks/
│   │   └── grafana-configs/
│   ├── reports/
│   │   ├── weekly-progress.md
│   │   └── monthly-summary.md
│   └── benchmarks/
│       ├── pipeline-performance.csv
│       └── security-scores.csv
│
├── 🧪 labs/
│   ├── security-testing/
│   │   ├── attack-scenarios/
│   │   └── defense-validation/
│   ├── tool-evaluation/
│   └── proof-of-concepts/
│
├── 📝 blog/
│   ├── drafts/
│   ├── published/
│   └── assets/
│       ├── images/
│       └── code-samples/
│
├── 🎬 demos/
│   ├── videos/
│   ├── presentations/
│   └── scripts/
│
├── 🔧 scripts/
│   ├── setup/
│   │   ├── install-tools.sh
│   │   └── configure-env.sh
│   ├── automation/
│   └── utilities/
│
├── 📦 resources/
│   ├── cheatsheets/
│   ├── templates/
│   └── references/
│
├── .gitignore
├── README.md
└── LICENSE
```

---

## 🏗️ Project Organization Best Practices

### 1. Naming Conventions

**Files**:
```
✅ Good:
- azure-pipelines.yml
- task-1.1-environment-setup.md
- security-scan-results-2024-12-26.json

❌ Bad:
- pipeline.yml
- notes.md
- scan.json
```

**Folders**:
```
✅ Good:
- completed-tasks/
- security-policies/
- terraform-modules/

❌ Bad:
- stuff/
- temp/
- old/
```

**Branches**:
```
✅ Good:
- feature/task-1.2-dotnet-api
- fix/pipeline-timeout
- security/implement-sast

❌ Bad:
- test
- backup
- final
```

---

### 2. Documentation Standards

#### Every project should have:

**README.md** template:
```markdown
# [Project Name]

## Overview
[1-2 paragraph description]

## Architecture
[Diagram or link to diagram]

## Security Features
- [Feature 1]
- [Feature 2]

## Prerequisites
- [Requirement 1]
- [Requirement 2]

## Setup
```bash
# Step-by-step commands
```

## Usage
[How to run/deploy]

## Testing
[How to test]

## Security Scanning
[How to run scans]

## Troubleshooting
[Common issues]

## Contributing
[Guidelines if applicable]
```

**SECURITY.md** template:
```markdown
# Security Policy

## Security Features
[List implemented security features]

## Vulnerability Reporting
[How to report issues]

## Security Scanning Results
[Link to latest scans]

## Compliance
[Standards adhered to]
```

---

### 3. Git Workflow

**Commit Message Convention**:
```
<type>(<scope>): <subject>

[optional body]

[optional footer]

Types:
- feat: New feature
- fix: Bug fix
- docs: Documentation
- security: Security improvement
- test: Testing
- chore: Maintenance

Examples:
feat(pipeline): add Trivy image scanning
security(api): implement rate limiting
docs(task-1.2): add setup instructions
```

**Branch Strategy**:
```
main
├── develop
│   ├── feature/task-1.1
│   ├── feature/task-1.2
│   └── security/sast-integration
└── hotfix/critical-cve
```

---

### 4. Security Artifacts Organization

**Store security scan results**:
```
security/scans/
├── 2024-12-26/
│   ├── trivy-scan.json
│   ├── trivy-scan.html
│   ├── sonar-report.pdf
│   └── snyk-results.json
├── 2024-12-27/
└── latest/ (symlink)
```

**SBOM management**:
```
security/sbom/
├── by-version/
│   ├── v1.0.0/
│   │   ├── sbom-cyclonedx.json
│   │   └── sbom-spdx.json
│   └── v1.1.0/
└── latest/ (symlink)
```

---

### 5. Pipeline Configuration Organization

**Modular pipelines**:
```yaml
# azure-pipelines.yml (main)
trigger:
  - main

stages:
  - template: pipelines/build-stage.yml
  - template: pipelines/security-stage.yml
  - template: pipelines/deploy-stage.yml

# pipelines/security-stage.yml (modular)
stages:
  - stage: SecurityScanning
    jobs:
      - template: jobs/sast-scan.yml
      - template: jobs/sca-scan.yml
      - template: jobs/container-scan.yml
```

---

### 6. Environment-Specific Configs

**Separate by environment**:
```
infrastructure/
├── environments/
│   ├── dev/
│   │   ├── terraform.tfvars
│   │   ├── backend.tf
│   │   └── config.yaml
│   ├── staging/
│   └── prod/
└── modules/ (shared)
```

---

### 7. Secrets Management

**Never commit**:
```
.gitignore should include:
# Secrets
*.key
*.pem
*.pfx
.env
secrets.yml
credentials.json

# Sensitive configs
*-secrets.yaml
*.credentials

# Azure
*.publishsettings
```

**Use**:
- Azure Key Vault
- GitHub Secrets
- Azure DevOps Variable Groups

---

### 8. Documentation Assets

**Organize diagrams**:
```
docs/architecture/diagrams/
├── source/
│   ├── architecture.drawio
│   ├── security-flow.plantuml
│   └── network-diagram.py (diagrams code)
├── rendered/
│   ├── architecture.png
│   ├── architecture.svg
│   └── security-flow.png
└── README.md (how to update diagrams)
```

---

### 9. Progress Tracking

**Weekly tracking file**:
```markdown
# Week 1 Progress (Dec 18-24, 2024)

## Tasks Completed
- [x] Task 1.1 - 4 hours
- [x] Task 1.2 - 7 hours
- [ ] Task 1.3 - 2/5 hours (in progress)

## Metrics
- Hours invested: 13/15 planned
- Tasks completed: 2/3
- Blockers: 1 (ACR permissions)

## Key Learnings
1. [Learning 1]
2. [Learning 2]

## Next Week
- Complete Task 1.3
- Start Task 2.1
```

---

### 10. Backup & Recovery

**What to backup**:
```
Important:
✅ Source code (Git)
✅ Documentation
✅ Configuration files
✅ Terraform state (remote backend)
✅ Security scan history

Not needed (regenerable):
❌ Build artifacts
❌ Docker images (in registry)
❌ Temporary files
```

---

## 🔄 Maintenance Schedule

### Daily
- [ ] Commit code changes
- [ ] Update task tracker
- [ ] Review security alerts

### Weekly
- [ ] Review Dependabot PRs
- [ ] Update weekly progress
- [ ] Clean up branches
- [ ] Backup important docs

### Monthly
- [ ] Security audit
- [ ] Update dependencies
- [ ] Review metrics
- [ ] Sprint retrospective
- [ ] Update roadmap

---

## 📱 Quick Reference Commands

### Git
```bash
# Start new task
git checkout -b feature/task-1.2

# Commit with security scan
git add .
git commit -m "security(api): add input validation"
pre-commit run --all-files

# Update from main
git checkout develop
git pull origin develop
git checkout feature/task-1.2
git rebase develop
```

### Azure
```bash
# Resource naming
rg-{project}-{env}-{region}
aks-{project}-{env}
acr{project}{env}

# Common commands
az group create --name rg-secureshop-dev-eastus
az acr create --name acrsecureshopdev
az aks get-credentials --name aks-secureshop-dev
```

### Documentation
```bash
# Create new task doc
cp task-completion-template.md \
   docs/completed-tasks/task-1.2-dotnet-api.md

# Generate diagram from code
python network-diagram.py
plantuml security-flow.plantuml
```

---

## 🎯 Organization Tips

1. **One task, one branch**
2. **Daily commits, even if small**
3. **Document as you go, not after**
4. **Clean up weekly, not monthly**
5. **Automate repetitive tasks**
6. **Version everything**
7. **Keep it simple, organized beats perfect**

---

**Created**: December 2024  
**Version**: 1.0  
**Maintained by**: [Your Name]
